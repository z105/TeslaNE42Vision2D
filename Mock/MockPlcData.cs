using System.IO;
using System.Text;

namespace TeslaNE42Vision2D.Mock
{
    public static class MockPlcData
    {
        public static byte[] CreateGoToStatePacket(string state)
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms, Encoding.ASCII);

            WritePaddedNullTerminatedString(writer, $"GoToState:{state}", 81);

            for (int i = 0; i < 10; i++)
                WritePaddedNullTerminatedString(writer, string.Empty, 81);

            for (int i = 0; i < 10; i++)
                writer.Write((double)0);

            return ms.ToArray();
        }

        public static byte[] CreateJobParametersPacket(string jobId)
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms, Encoding.ASCII);

            WritePaddedNullTerminatedString(writer, $"JobParameters:{jobId}", 81);

            for (int i = 0; i < 10; i++)
                WritePaddedNullTerminatedString(writer, string.Empty, 81);

            for (int i = 0; i < 10; i++)
                writer.Write((double)0);

            return ms.ToArray();
        }

        private static void WritePaddedNullTerminatedString(BinaryWriter writer, string value, int length)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            if (bytes.Length >= length)
            {
                writer.Write(bytes, 0, length);
            }
            else
            {
                writer.Write(bytes);
                writer.Write((byte)0);
                for (int i = bytes.Length + 1; i < length; i++)
                    writer.Write((byte)0);
            }
        }
    }
}
