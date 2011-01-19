using System.Collections.Generic;

namespace Caliberweb.Core.IO.Csv
{
    internal class CsvRecord : ICsvRecord
    {
        public CsvRecord(IEnumerable<ICsvValue> values)
        {
            Values = new CsvValueList(values);
        }

        public CsvValueList Values { get; private set; }

        public int CompareTo(ICsvRecord other)
        {
            return 0;
        }
    }
}