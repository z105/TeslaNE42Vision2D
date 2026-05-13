using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TeslaNE42Vision2D.Packet
{
    public class ByteArrayFactory
    {
        private const int DefaultStringSize = 252;

        private readonly List<byte[]> _data = new List<byte[]>();

        public void Append<T>(T? data) where T : struct
        {
            if (!data.HasValue)
            {
                _data.Add(new byte[Marshal.SizeOf<T>()]);
                return;
            }

            switch (data)
            {
                case byte byteData:
                    _data.Add(new byte[] { byteData });
                    break;
                case sbyte sbyteData:
                    _data.Add(new byte[] { (byte)sbyteData });
                    break;
                case short shortData:
                    _data.Add(BitConverter.GetBytes(shortData));
                    break;
                case ushort ushortData:
                    _data.Add(BitConverter.GetBytes(ushortData));
                    break;
                case int intData:
                    _data.Add(BitConverter.GetBytes(intData));
                    break;
                case uint uintData:
                    _data.Add(BitConverter.GetBytes(uintData));
                    break;
                case long longData:
                    _data.Add(BitConverter.GetBytes(longData));
                    break;
                case ulong ulongData:
                    _data.Add(BitConverter.GetBytes(ulongData));
                    break;
                case float floatData:
                    _data.Add(BitConverter.GetBytes(floatData));
                    break;
                case double doubleData:
                    _data.Add(BitConverter.GetBytes(doubleData));
                    break;
                case bool boolData:
                    _data.Add(BitConverter.GetBytes(boolData));
                    break;
                default:
                    throw new InvalidOperationException($"Type {typeof(T)} is not supported.");
            }
        }

        public void AppendString(string data, int size)
        {
            _data.Add(StringToBytes(data, size));
        }

        public void Append(byte[] data)
        {
            _data.Add(data);
        }

        private static byte[] StringToBytes(string input, int arraySize = DefaultStringSize)
        {
            if (input == null) input = string.Empty;
            if (input.Length > arraySize) input = input.Substring(0, arraySize);

            byte[] outputBytes = new byte[arraySize + 1];
            Encoding.ASCII.GetBytes(input).CopyTo(outputBytes, 0);
            return outputBytes;
        }

        public byte[] GenerateByteArray()
        {
            return ConcatenateByteArrays(_data.ToArray());
        }

        public static byte[] ConcatenateByteArrays(params byte[][] arrays)
        {
            int totalLen = 0;
            foreach (var a in arrays) totalLen += a.Length;
            byte[] result = new byte[totalLen];
            int offset = 0;
            foreach (var array in arrays)
            {
                Buffer.BlockCopy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }
            return result;
        }
    }
}
