using System;

namespace Caliberweb.Core.IO.Csv
{
    public interface ICsvRecord : IComparable<ICsvRecord>
    {
        CsvValueList Values { get; }
    }
}