using Polly;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace TeslaNE42Vision2D.SocketTCPClient
{
    public class RequestEventArgs : EventArgs
    {
        public byte[] Info { get; }
        public string Key { get; }

        public RequestEventArgs(byte[] info, string key)
        {
            Info = info;
            Key = key;
        }
    }

    public class SocketTCPClient
    {
        public struct ClientReceiveData
        {
            public byte[] Buffer;
            public int OldTotalBufferLength;
            public int TimeOutCount;
        }

        public string m_ServerIP;
        public int m_ServerPort;
        private readonly int m_BufferLength = 16192;
        private const int COMMAND_PACKET_BYTE_LENGTH = 971;

        private Socket m_Socket = null;
        private SocketAsyncEventArgs m_Receive_SocketAsyncEventArgs;
        private IAsyncPolicy _retryPolicy;
        private bool _isReconnecting = false;

        public delegate void OnEventCompletedHandler(object sender, object e);
        public delegate void RequestEventHandler(object sender, RequestEventArgs e);

        public event OnEventCompletedHandler OnConnectedEvent;
        public event OnEventCompletedHandler OnDisconnectEvent;
        public event OnEventCompletedHandler OnExceptionEvent;
        public event RequestEventHandler OnReceiveCompletedEvent;

        public SocketTCPClient()
        {
            InitializeRetryPolicy();
        }

        public SocketTCPClient(string serverIP, int serverPort)
        {
            m_ServerIP = serverIP;
            m_ServerPort = serverPort;
            InitializeRetryPolicy();
        }

        private void InitializeRetryPolicy()
        {
            // 无限重试，直到连接成功
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(1),
                    onRetry: (exception, timespan) =>
                    {
                        OnExceptionEvent?.Invoke(this, $"连接失败，1秒后重试: {exception.Message}");
                    }
                );
        }

        public bool IsConnected
        {
            get
            {
                if (m_Socket == null) return false;
                return m_Socket.Connected;
            }
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    Close();

                    m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                    {
                        NoDelay = true,
                        ReceiveBufferSize = m_BufferLength,
                        SendBufferSize = m_BufferLength,
                    };

                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(m_ServerIP), m_ServerPort);
                    await m_Socket.ConnectAsync(endPoint);

                    byte[] receiveBuffer = new byte[m_BufferLength];
                    m_Receive_SocketAsyncEventArgs = new SocketAsyncEventArgs();
                    m_Receive_SocketAsyncEventArgs.UserToken = new ClientReceiveData();
                    m_Receive_SocketAsyncEventArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                    m_Receive_SocketAsyncEventArgs.Completed += Receive_Completed;
                    m_Receive_SocketAsyncEventArgs.AcceptSocket = m_Socket;
                    m_Socket.ReceiveAsync(m_Receive_SocketAsyncEventArgs);
                });

                OnConnectedEvent?.Invoke(this, m_ServerIP);
                return true;
            }
            catch (Exception ex)
            {
                OnExceptionEvent?.Invoke(this, "连接服务器失败：" + ex.Message);
                return false;
            }
        }

        public bool Connect()
        {
            return ConnectAsync().GetAwaiter().GetResult();
        }

        private async Task AutoReconnectAsync()
        {
            if (_isReconnecting) return;
            _isReconnecting = true;
            try
            {
                OnExceptionEvent?.Invoke(this, "连接已断开，正在尝试重连...");
                await ConnectAsync();
            }
            finally
            {
                _isReconnecting = false;
            }
        }

        private void Receive_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Receive) return;

            int lengthBuffer = e.BytesTransferred;
            if (lengthBuffer <= 0)
            {
                OnDisconnectEvent?.Invoke(this, m_ServerIP);
                Close();
                return;
            }

            SocketAsyncReceiveBuffer(e);

            try
            {
                // 读取局部引用以避免竞态
                var sock = e.AcceptSocket;
                var currentArgs = Volatile.Read(ref m_Receive_SocketAsyncEventArgs);

                // 仅在 socket 存在且仍是当前使用的 args 时重用
                if (sock != null && sock.Connected && object.ReferenceEquals(currentArgs, e))
                {
                    bool pending = sock.ReceiveAsync(e);
                    if (!pending)
                    {
                        // 同步完成，手动调用处理
                        Receive_Completed(this, e);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // socket 已被关闭/释放，安全忽略或记录
            }
            catch (Exception ex)
            {
                OnExceptionEvent?.Invoke(this, "接收循环异常：" + ex.Message);
            }
        }

        private void SocketAsyncReceiveBuffer(SocketAsyncEventArgs e)
        {
            ClientReceiveData clientData = (ClientReceiveData)e.UserToken;
            try
            {
                string key = e.AcceptSocket.RemoteEndPoint.ToString();
                int length = e.BytesTransferred;

                if (clientData.Buffer == null)
                {
                    clientData.Buffer = new byte[COMMAND_PACKET_BYTE_LENGTH];
                    clientData.OldTotalBufferLength = 0;
                    Buffer.BlockCopy(e.Buffer, 0, clientData.Buffer, 0, length);
                    clientData.OldTotalBufferLength += length;

                    if (length == COMMAND_PACKET_BYTE_LENGTH)
                    {
                        OnReceiveCompletedEvent?.Invoke(this, new RequestEventArgs(clientData.Buffer, key));
                        clientData.Buffer = null;
                    }
                    e.UserToken = clientData;
                }
                else
                {
                    if (COMMAND_PACKET_BYTE_LENGTH == clientData.OldTotalBufferLength + length)
                    {
                        Buffer.BlockCopy(e.Buffer, 0, clientData.Buffer, clientData.OldTotalBufferLength, length);
                        clientData.OldTotalBufferLength += length;
                        OnReceiveCompletedEvent?.Invoke(this, new RequestEventArgs(clientData.Buffer, key));
                        clientData.Buffer = null;
                    }
                    else if (COMMAND_PACKET_BYTE_LENGTH > clientData.OldTotalBufferLength + length)
                    {
                        Buffer.BlockCopy(e.Buffer, 0, clientData.Buffer, clientData.OldTotalBufferLength, length);
                        clientData.OldTotalBufferLength += length;
                    }
                    else
                    {
                        clientData.Buffer = null;
                        OnExceptionEvent?.Invoke(this, "数据超出缓存区大小，已丢弃");
                    }
                    e.UserToken = clientData;
                }
            }
            catch (Exception ex)
            {
                clientData.Buffer = null;
                e.UserToken = clientData;
                OnExceptionEvent?.Invoke(this, "数据接收异常：" + ex.Message);
            }
        }

        public async Task<bool> SendAsync(byte[] sendBuffer)
        {
            try
            {
                if (!IsConnected)
                {
                    bool reconnected = await ConnectAsync();
                    if (!reconnected) return false;
                }
                await m_Socket.SendAsync(new ArraySegment<byte>(sendBuffer), SocketFlags.None);
                return true;
            }
            catch (Exception ex)
            {
                OnExceptionEvent?.Invoke(this, "发送数据失败：" + ex.Message);
                _ = AutoReconnectAsync();
                return false;
            }
        }

        public bool Send(byte[] sendBuffer)
        {
            m_Socket.Send(sendBuffer);
            return true;
            //return SendAsync(sendBuffer).GetAwaiter().GetResult();
        }

  

        public void Disconnect()
        {
            if (m_Socket != null && m_Socket.Connected)
                m_Socket.Disconnect(true);
            OnDisconnectEvent?.Invoke(this, m_ServerIP);
        }

        public void Close()
        {
            // 原子交换，防止并发回调继续使用旧引用
            var args = Interlocked.Exchange(ref m_Receive_SocketAsyncEventArgs, null);
            if (args != null)
            {
                try { args.Completed -= Receive_Completed; } catch { }
                try { args.AcceptSocket = null; } catch { }
                try { args.Dispose(); } catch { }
            }

            var s = Interlocked.Exchange(ref m_Socket, null);
            if (s != null)
            {
                try { s.Shutdown(SocketShutdown.Both); } catch { }
                try { s.Close(); } catch { }
                try { s.Dispose(); } catch { }
            }
        }
    }
}
