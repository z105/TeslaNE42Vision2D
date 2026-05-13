namespace TeslaNE42Vision2D.Packet
{
    public class CommandResponseTcpPacket : TcpPacketBase
    {
        public string Command { get; set; }
        public bool IsSuccessful { get; set; }
        public string ResponseMessage { get; set; }

        public CommandResponseTcpPacket(string visionStateMachineId)
            : base(visionStateMachineId)
        {
            TcpPacketType = (short)TcpPacketTypeEnum.CommandResponse;
        }

        protected override byte[] ToByteArray()
        {
            ByteArrayFactory factory = new ByteArrayFactory();
            factory.Append<short>(TcpPacketType);
            factory.AppendString(Hostname, 20);
            factory.AppendString(VisionStateMachineId, 4);
            factory.AppendString(Command, 80);
            factory.Append<byte>((byte)(IsSuccessful ? 1 : 0));
            factory.AppendString(ResponseMessage, 160);
            return factory.GenerateByteArray();
        }
    }

    public enum TcpPacketTypeEnum
    {
        Status = 1,
        Results = 2,
        InputRequest = 3,
        VariableLengthResults = 4,
        CommandResponse = 5,
    }
}
