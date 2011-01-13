using System.Collections.Generic;

namespace Caliberweb.Core
{
    public interface IByteCodec
    {
        string Encode(IEnumerable<byte> bytes);
        IEnumerable<byte> Decode(string text);
    }
}