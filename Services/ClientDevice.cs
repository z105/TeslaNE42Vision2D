using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Packet;
using TeslaNE42Vision2D.Services.Vision;
using TeslaNE42Vision2D.SocketTCPClient;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Services
{
    public class ClientDevice
    {
        private readonly SocketTCPClient.SocketTCPClient _tcpClient;
        private readonly AppStateMachine _stateMachine;
        private StatusTcpPacket _statusPacket;
        private readonly FifoSemaphore _statusSendSemaphore = new FifoSemaphore(1);
        private CyclicTask _heartbeat;
        private TimeoutHandler _heartbeatTimeout;

        private string _serverIp;
        private int _serverPort;
        private string _machineId;
        private int _jobId;
        private readonly List<string[]> _jobStringParams = new List<string[]>();
        private readonly List<double[]> _jobDoubleParams = new List<double[]>();
        private readonly ManualResetEvent[] _jobInputEvents =
            Enumerable.Range(0, 1000).Select(_ => new ManualResetEvent(false)).ToArray();

        private const int HEARTBEAT_DELAY_MS = 1000;
        private const int HEARTBEAT_TIMEOUT_MS = 3000;
        private const int RECONNECT_INTERVAL_MS = 3000;
        private bool _autoReconnect = true;
        private bool _isReconnecting = false;

        private ulong _heartbeatReceiveCount = 0;
        private bool _heartbeatTimeoutFlag = false;

        public event Action<string> OnLogEvent;
        public event Action<DetectResultEntity> OnDetectResultEvent;
        public event Action<bool> OnConnectionChanged;
        public event Action<ulong, bool> OnHeartbeatStatusChanged;

        public ulong HeartbeatReceiveCount => _heartbeatReceiveCount;
        public bool HeartbeatTimeoutFlag => _heartbeatTimeoutFlag;

        public bool IsConnected => _tcpClient.IsConnected;
        public MachineState? State => _stateMachine?.State;
        public AppStateMachine StateMachine => _stateMachine;

        public ClientDevice(string serverIp, int serverPort, string machineId)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
            _machineId = machineId;

            _tcpClient = new SocketTCPClient.SocketTCPClient(serverIp, serverPort);
            _tcpClient.OnReceiveCompletedEvent += OnReceiveCompleted;
            _tcpClient.OnConnectedEvent += (s, e) =>
            {
                WriteLog("PLC 连接成功");
                OnConnectionChanged?.Invoke(true);
                _isReconnecting = false;
            };
            _tcpClient.OnDisconnectEvent += (s, e) =>
            {
                WriteLog("PLC 连接断开");
                OnConnectionChanged?.Invoke(false);
                if (_autoReconnect && !_isReconnecting)
                {
                    System.Threading.Tasks.Task.Run(async () => await TryReconnectLoop());
                }
            };

            _tcpClient.OnExceptionEvent += (s, e) => WriteLog(e?.ToString() ?? "TCP异常");

            _stateMachine = new AppStateMachine();
            _stateMachine.OnLogEvent += msg => WriteLog(msg);
            _stateMachine.OnStateChanged += OnStateMachineStateChanged;
            _stateMachine.OnDetectResultEvent += result => OnDetectResultEvent?.Invoke(result);

            for (int i = 0; i < 100; i++)
            {
                _jobStringParams.Add(new string[1000]);
                _jobDoubleParams.Add(new double[1000]);
            }

            _heartbeat = new CyclicTask(SendHeartbeat, TimeSpan.FromMilliseconds(HEARTBEAT_DELAY_MS));
            _heartbeatTimeout = new TimeoutHandler(
                TimeSpan.FromMilliseconds(HEARTBEAT_TIMEOUT_MS),
                () =>
                {
                    _heartbeatTimeoutFlag = true;
                    WriteLog("PLC 心跳超时！");
                    OnHeartbeatStatusChanged?.Invoke(_heartbeatReceiveCount, true);
                    // 心跳超时时主动断开连接，触发重连机制
                    if (_autoReconnect && !_isReconnecting)
                    {
                        _heartbeat.Stop();
                        _heartbeatTimeout.Stop();
                        _tcpClient.Close();
                        OnConnectionChanged?.Invoke(false);
                        System.Threading.Tasks.Task.Run(async () => await TryReconnectLoop());
                    }
                });
        }

        private void OnStateMachineStateChanged(MachineState from, MachineState to, StateEvent evt)
        {
            if (_statusPacket == null) return;
            _statusPacket.State = (short)to;
            if (evt == StateEvent.Reset)
                _statusPacket.ClearError();
            WritePacket(_statusPacket);
        }

        public async System.Threading.Tasks.Task<bool> ConnectAsync()
        {
            _statusPacket = new StatusTcpPacket(_machineId);
            bool ok = await _tcpClient.ConnectAsync();
            if (ok)
            {
                _heartbeat.Start();
                _heartbeatTimeout.Start();
            }
            return ok;
        }

        public bool Connect()
        {
            return ConnectAsync().GetAwaiter().GetResult();
        }

        public bool Reconnect()
        {
            return ConnectAsync().GetAwaiter().GetResult();
        }

        public void Disconnect()
        {
            _autoReconnect = false;
            _isReconnecting = false;
            _heartbeat.Stop();
            _heartbeatTimeout.Stop();
            _tcpClient?.Disconnect();
        }

        private async System.Threading.Tasks.Task TryReconnectLoop()
        {
            _isReconnecting = true;
            while (_autoReconnect && !IsConnected)
            {
                WriteLog("尝试重新连接 PLC...");
                try
                {
                    bool success = await ConnectAsync();
                    if (success)
                    {
                        WriteLog("PLC 重连成功");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog($"重连异常: {ex.Message}");
                }
                await System.Threading.Tasks.Task.Delay(RECONNECT_INTERVAL_MS);
            }
            _isReconnecting = false;
        }

        private void SendHeartbeat()
        {
            if (!IsConnected) return;
            _statusSendSemaphore.Wait();
            try
            {
                if (_statusPacket != null)
                {
                    _statusPacket.State = (short)(_stateMachine?.State ?? MachineState.Preoperational);
                    WritePacket(_statusPacket);
                }
            }
            finally
            {
                _statusSendSemaphore.Release();
            }
        }

        private void OnReceiveCompleted(object sender, RequestEventArgs e)
        {
            ParseAndProcessPacket(e.Info);
        }

        public void ParseAndProcessPacket(byte[] packetBytes)
        {
            CommandTcpPacket commandPacket;
            try
            {
                commandPacket = new CommandTcpPacket(packetBytes);
            }
            catch (Exception ex)
            {
                WriteLog("解析数据包异常: " + ex.Message);
                SendCommandResponse(string.Empty, false, "Failed to parse TCP command");
                return;
            }

            if (commandPacket.Command == "Heartbeat")
            {
                _heartbeatReceiveCount++;
                _heartbeatTimeoutFlag = false;
                _heartbeatTimeout.Restart();
                OnHeartbeatStatusChanged?.Invoke(_heartbeatReceiveCount, false);
                return;
            }

            WriteLog("收到命令: " + commandPacket.Command);

            try
            {
                if (commandPacket.Command.StartsWith("JobParameters"))
                {
                    string[] parts = commandPacket.Command.Split(':');
                    if (parts.Length > 1 && int.TryParse(parts[1], out int jobId) && jobId < _jobStringParams.Count)
                    {
                        _jobStringParams[jobId] = commandPacket.StringCommandParameters;
                        _jobDoubleParams[jobId] = commandPacket.LRealCommandParameters;
                        if (jobId < _jobInputEvents.Length)
                            _jobInputEvents[jobId].Set();
                        _jobId = jobId;
                    }
                    return;
                }

                LogHelper.Info($"command: {commandPacket.Command}. stringList: {string.Join(",", commandPacket.StringCommandParameters)}. doubleList: {string.Join(",", commandPacket.LRealCommandParameters) }");

                ProcessCommand(commandPacket.Command, commandPacket.StringCommandParameters, commandPacket.LRealCommandParameters);

                //if(commandPacket.Command == "GoToState:SingleExecution")
                //{
                //    DetectResultEntity result = new DetectResultEntity();
                //    result.OkNg = true;
                //    WriteLog($"jobid: {_jobId}");
                //    RunDataService.Instance.ClientDevice.SendDetectResult(result);
                //}
                // 发送检测结果（示例）
                //VisionJob job = new VisionJob
                //{
                //    Id = _jobId,
                //    Assessment = VisionJob.AssessmentEnum.OK,
                //    Status = VisionJob.StatusEnum.Completed,
                //    StatusMessage = "OK",
                //};
                //WriteResults(job, null, null, null);


                //SendCommandResponse(commandPacket.Command, true, "Success");
            }
            catch (Exception ex)
            {
                WriteLog("处理命令异常: " + ex.Message);
                SendCommandResponse(commandPacket.Command, false, ex.Message);
            }
        }

        private void ProcessCommand(string command, string[] stringParams, double[] doubleParams)
        {
            if (command.StartsWith("GoToState"))
            {
                string[] parts = command.Split(':');
                if (parts.Length < 2) return;
                string targetState = parts[1];

                switch (targetState)
                {
                    case "Preoperational":
                        WriteLog("收到 Preoperational 请求");
                        _stateMachine.Trigger(StateEvent.Reset);
                        break;
                    case "Ready":
                        WriteLog("收到 Ready 请求");
                        _stateMachine.Trigger(StateEvent.PrepareRecipe);
                        break;
                    case "SingleExecution":
                        WriteLog("收到 SingleExecution 请求");
                        _stateMachine.TriggerSingleJob(stringParams, doubleParams);
                        break;
                    case "ContinuousExecution":
                        WriteLog("收到 ContinuousExecution 请求");
                        _stateMachine.TriggerContinuous();
                        break;
                    case "Halt":
                        WriteLog("收到 Halt 请求");
                        _stateMachine.Trigger(StateEvent.Halt);
                        break;
                }

                _statusSendSemaphore.Wait();
                try
                {
                    if (_statusPacket != null)
                    {
                        _statusPacket.State = (short)(_stateMachine?.State ?? MachineState.Preoperational);
                        LogHelper.Info("Machine State:" + _stateMachine?.State.ToString());
                        WritePacket(_statusPacket);
                    }
                }
                finally
                {
                    _statusSendSemaphore.Release();
                }
                //_heartbeat.Restart();
            }
            else if (command.StartsWith("SetJobId"))
            {
                string[] parts = command.Split(':');
                if (parts.Length > 1 && int.TryParse(parts[1], out int id))
                    _jobId = id;
            }
        }

        public bool WriteResults(VisionJob job, string[] stringResults, double[] doubleResults, long[] longResults)
        {
            ResultsTcpPacket packet = new ResultsTcpPacket(_machineId)
            {
                JobId = (short)(job?.Id ?? 0),
                JobAssessment = (short)(job?.Assessment ?? VisionJob.AssessmentEnum.NC),
                JobStatus = (short)(job?.Status ?? VisionJob.StatusEnum.Completed),
                JobStatusMessage = job?.StatusMessage ?? string.Empty,
            };
            packet.JobResultString = stringResults;
            packet.JobResultLReal = doubleResults;
            packet.JobResultLInt = longResults;
            //if (stringResults != null)
            //    for (int i = 0; i < Math.Min(stringResults.Length, ResultsTcpPacket.DoubleArraySize); i++)
            //        packet.JobResultString[i] = stringResults[i] ?? string.Empty;

            //if (doubleResults != null)
            //    for (int i = 0; i < Math.Min(doubleResults.Length, ResultsTcpPacket.DoubleArraySize); i++)
            //        packet.JobResultLReal[i] = doubleResults[i];

            //if (longResults != null)
            //    for (int i = 0; i < Math.Min(longResults.Length, ResultsTcpPacket.DoubleArraySize); i++)
            //        packet.JobResultLInt[i] = longResults[i];

            return WritePacket(packet);
        }

        // 发送视觉检测结果到PLC（带坐标）
        public bool SendDetectResult(DetectResultEntity result, PositionType positionType = PositionType.All)
        {
            VisionJob job = new VisionJob
            {
                Id = _jobId,
                Assessment = result.OkNg ? VisionJob.AssessmentEnum.OK : VisionJob.AssessmentEnum.NG,
                Status = VisionJob.StatusEnum.Completed,
                StatusMessage = result.OkNg ? "OK" : "NG",
            };

            List<string> barcodeList = result.InspectionOutputs.OrderByDescending(r => r.Index)
                .SelectMany(r => r.BarcodeList)
                .ToList();


            List<long> polarityList = result.InspectionOutputs.OrderByDescending(r => r.Index)
                .SelectMany(s => s.PolarityList)
                .Select(p => (long)p)
                .ToList();


            string[] strings = GetBarcodeList(positionType, barcodeList.ToArray());
            double[] doubles = new double[10];
            long[] t_longs = GetPolarityList(positionType, polarityList.ToArray());

            doubles[0] = result.PhysicalX;
            doubles[1] = result.PhysicalY;
            doubles[2] = result.PhysicalAngle;
            doubles[3] = result.OkNg ? 1 : 0;

            return WriteResults(job, strings, doubles, t_longs);
        }

        public bool SendError(string errorMessage)
        {
            VisionJob job = new VisionJob
            {
                Id = _jobId,
                Assessment = VisionJob.AssessmentEnum.NC,
                Status = VisionJob.StatusEnum.Failed,
                StatusMessage = errorMessage,
            };
            return WriteResults(job, null, null, null);
        }

        public long[] GetPolarityList(PositionType positionType, long[] polarity)
        {

            long[] result = new long[24];

            switch (positionType)
            {
                case PositionType.Left:
                    // 填充前 12 位 (索引 0-11)
                    // 取传入数组的前 12 个元素（如果不足 12 个则取全部），防止数组越界
                    int leftLength = Math.Min(polarity.Length, 12);
                    Array.Copy(polarity, 0, result, 12, leftLength);
                    break;

                case PositionType.Right:
                    // 填充后 12 位 (索引 12-23)
                    int rightLength = Math.Min(polarity.Length, 12);
                    Array.Copy(polarity, 0, result, 0, rightLength);

                    break;

                case PositionType.All:
                    // 填充所有 24 位 (索引 0-23)
                    int allLength = Math.Min(polarity.Length, 24);
                    Array.Copy(polarity, 0, result, 0, allLength);
                    break;

                default:
                    break;
            }

            return result;
        }

        public string[] GetBarcodeList(PositionType positionType, string[] barcode)
        {

            string[] result = new string[24];

            switch (positionType)
            {
                case PositionType.Left:
                    // 填充前 12 位 (索引 0-11)
                    // 取传入数组的前 12 个元素（如果不足 12 个则取全部），防止数组越界
                    int leftLength = Math.Min(barcode.Length, 12);
                    Array.Copy(barcode, 0, result, 12, leftLength);
                    break;

                case PositionType.Right:
                    // 填充后 12 位 (索引 12-23)
                    int rightLength = Math.Min(barcode.Length, 12);
                    Array.Copy(barcode, 0, result, 0, rightLength);
                    break;

                case PositionType.All:
                    // 填充所有 24 位 (索引 0-23)
                    int allLength = Math.Min(barcode.Length, 24);
                    Array.Copy(barcode, 0, result, 0, allLength);
                    break;

                default:
                    break;
            }

            return result;
        }

        private void SendCommandResponse(string command, bool isSuccessful, string message)
        {
            // WriteLog("收到 Halt 请求");
            WritePacket(new CommandResponseTcpPacket(_machineId)
            {
                Command = command,
                IsSuccessful = isSuccessful,
                ResponseMessage = message,
            });
        }

        private bool WritePacket(TcpPacketBase packet)
        {
            if (packet is CommandResponseTcpPacket)
            {
                return false;
            }

            //LogHelper.Info("TcpPacketType:" + packet.TcpPacketType);
            if (!IsConnected) return false;
            return _tcpClient.Send(packet.ByteArray);
        }

        public void SetStateMachineServices(
            List<Camera.ICameraService> cameras,
            Dictionary<string, IVisionService> visionServices,
            Calibration.NinePointCalibrationVisionProService calibrationService)
        {
            _stateMachine.Cameras = cameras;
            _stateMachine.VisionServices = visionServices;
            _stateMachine.CalibrationService = calibrationService;
        }

        private void WriteLog(string message)
        {
            LogHelper.Info(message);
            OnLogEvent?.Invoke(message);
        }

        public void Dispose()
        {
            _heartbeat?.Stop();
            _heartbeatTimeout?.Dispose();
            _tcpClient?.Close();
        }
    }
}
