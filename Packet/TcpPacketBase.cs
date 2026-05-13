using System;
using System.Net;

namespace TeslaNE42Vision2D.Packet
{
    public abstract class TcpPacketBase
    {
        private string _visionStateMachineId;

        public TcpPacketBase(string visionStateMachineId)
        {
            _visionStateMachineId = visionStateMachineId;
        }

        public short TcpPacketType { get; protected set; }

        public string Hostname { get; private set; } = Dns.GetHostName();

        public string VisionStateMachineId
        {
            get => _visionStateMachineId;
            set => _visionStateMachineId = value;
        }

        public byte[] ByteArray => ToByteArray();

        protected abstract byte[] ToByteArray();
    }
}
