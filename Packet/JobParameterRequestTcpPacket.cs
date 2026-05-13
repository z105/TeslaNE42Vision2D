namespace TeslaNE42Vision2D.Packet
{
    public class JobParameterRequestTcpPacket : TcpPacketBase
    {
        public short JobId { get; set; }

        public JobParameterRequestTcpPacket(string visionStateMachineId)
            : base(visionStateMachineId)
        {
            TcpPacketType = (short)TcpPacketTypeEnum.InputRequest;
        }

        protected override byte[] ToByteArray()
        {
            ByteArrayFactory factory = new ByteArrayFactory();
            factory.Append<short>(TcpPacketType);
            factory.AppendString(Hostname, 20);
            factory.AppendString(VisionStateMachineId, 4);
            factory.Append<short>(JobId);
            return factory.GenerateByteArray();
        }
    }
}
