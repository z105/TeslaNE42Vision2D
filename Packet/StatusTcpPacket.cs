namespace TeslaNE42Vision2D.Packet
{
    public class StatusTcpPacket : TcpPacketBase
    {
        public short State { get; set; } = -1;
        public string ActiveDataflow { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public short ErrorSeverity { get; set; } = (short)VisionErrorSeverities.None;

        public StatusTcpPacket(string visionStateMachineId)
            : base(visionStateMachineId)
        {
            TcpPacketType = (short)TcpPacketTypeEnum.Status;
        }

        public void ClearError()
        {
            ErrorMessage = string.Empty;
            ErrorSeverity = (short)VisionErrorSeverities.None;
        }

        protected override byte[] ToByteArray()
        {
            ByteArrayFactory factory = new ByteArrayFactory();
            factory.Append<short>(TcpPacketType);
            factory.AppendString(Hostname, 20);
            factory.AppendString(VisionStateMachineId, 4);
            factory.Append<short>(State);
            factory.AppendString(ActiveDataflow, 80);
            factory.AppendString(ErrorMessage, 160);
            factory.Append<short>(ErrorSeverity);
            return factory.GenerateByteArray();
        }
    }

    public enum VisionErrorSeverities
    {
        None,
        Warning,
        Error,
        Critical,
    }
}
