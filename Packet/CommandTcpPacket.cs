using System;
using System.IO;
using System.Text;

namespace TeslaNE42Vision2D.Packet
{
    public class CommandTcpPacket
    {
        public string Command { get; }
        public string[] StringCommandParameters { get; }
        public double[] LRealCommandParameters { get; }

        public CommandTcpPacket(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            BinaryReader reader = new BinaryReader(ms, Encoding.ASCII);

            Command = ReadString(reader, 81);

            StringCommandParameters = new string[10];
            for (int i = 0; i < 10; i++)
                StringCommandParameters[i] = ReadString(reader, 81);

            LRealCommandParameters = new double[10];
            for (int i = 0; i < 10; i++)
                LRealCommandParameters[i] = reader.ReadDouble();
        }

        private string ReadString(BinaryReader reader, int length)
        {
            byte[] bytes = reader.ReadBytes(length);
            int nullIndex = Array.IndexOf(bytes, (byte)0);
            if (nullIndex >= 0)
                return Encoding.ASCII.GetString(bytes, 0, nullIndex);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
