using System;

namespace Caliberweb.Core.IO
{
    public interface ICsvRecord : IComparable<ICsvRecord>
    {
        CsvValueList Values { get; }
    }
}