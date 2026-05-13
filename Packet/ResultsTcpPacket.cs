using System;

namespace TeslaNE42Vision2D.Packet
{
    public class ResultsTcpPacket : TcpPacketBase
    {
        public const short DoubleArraySize = 10;

        public const short StringArraySize = 24;

        public const short LIntArraySize = 24;


        public short JobId { get; set; }
        public short JobAssessment { get; set; }
        public short JobStatus { get; set; }
        public string JobStatusMessage { get; set; }

        protected string[] _jobResultString = new string[StringArraySize];
        public virtual string[] JobResultString
        {
            get => _jobResultString;
            set
            {
                _jobResultString = value;
            }
        }

        protected double[] _jobResultLReal = new double[DoubleArraySize];
        public virtual double[] JobResultLReal
        {
            get => _jobResultLReal;
            set
            {
                _jobResultLReal = value;
            }
        }

        protected long[] _jobResultLInt = new long[LIntArraySize];
        public virtual long[] JobResultLInt
        {
            get => _jobResultLInt;
            set
            {
                _jobResultLInt = value;
            }
        }

        public ResultsTcpPacket(string visionStateMachineId)
            : base(visionStateMachineId)
        {
            TcpPacketType = (short)TcpPacketTypeEnum.Results;
        }

        protected override byte[] ToByteArray()
        {
            ByteArrayFactory factory = new ByteArrayFactory();
            factory.Append<short>(TcpPacketType);
            factory.AppendString(Hostname, 20);
            factory.AppendString(VisionStateMachineId, 4);
            factory.Append<short>(JobId);
            factory.Append<short>(JobAssessment);
            factory.Append<short>(JobStatus);
            factory.AppendString(JobStatusMessage, 160);

            for (int i = 0; i < JobResultString.Length; i++)
                factory.AppendString(JobResultString[i] ?? string.Empty, 80);

            for (int i = 0; i < JobResultLReal.Length; i++)
                factory.Append<double>(JobResultLReal[i]);

            for (int i = 0; i < JobResultLInt.Length; i++)
                factory.Append<long>(JobResultLInt[i]);

            return factory.GenerateByteArray();
        }
    }
}
