using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Caliberweb.Core
{
    public class ByteCodec : IByteCodec
    {
        public static IByteCodec Base64 = new ByteCodec();
        public static IByteCodec Hex = new HexByteCodec();
        public static IByteCodec Null = new NullByteCodec();

        private ByteCodec()
        {}

        public string Encode(IEnumerable<byte> bytes)
        {
            return Convert.ToBase64String(bytes.ToArray());
        }

        public IEnumerable<byte> Decode(string text)
        {
            return Convert.FromBase64String(text);
        }

        private class HexByteCodec : IByteCodec
        {
            public string Encode(IEnumerable<byte> bytes)
            {
                return bytes.Aggregate("", (s, b) => string.Concat(s, b.ToString("X2")));
            }

            public IEnumerable<byte> Decode(string text)
            {
                int length = text.Length;
                var bytes = new byte[length / 2];
                for (int i = 0; i < length; i += 2)
                    bytes[i / 2] = Convert.ToByte(text.Substring(i, 2), 16);
                return bytes;
            }
        }

        private class NullByteCodec : IByteCodec
        {
            public string Encode(IEnumerable<byte> bytes)
            {
                var array = bytes.ToArray();

                using (var stream = new MemoryStream())
                using (var reader = new StreamReader(stream))
                {
                    stream.Write(array, 0, array.Length);
                    stream.Flush();
                    stream.Position = 0;

                    var text = reader.ReadToEnd();

                    return text;
                }
            }

            public IEnumerable<byte> Decode(string text)
            {
                return Encoding.UTF8.GetBytes(text);
            }
        }
    }
}