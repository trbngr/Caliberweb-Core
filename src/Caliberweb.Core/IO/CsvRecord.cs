using System;
using System.Collections.Generic;
using System.Linq;

namespace Caliberweb.Core.IO
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

    public class CsvValueList : List<ICsvValue>
    {
        private const StringComparison COMPARISON = StringComparison.InvariantCultureIgnoreCase;

        public CsvValueList(IEnumerable<ICsvValue> collection) : base(collection)
        {}

        public T GetColumnValue<T>(IColumn<T> column)
        {
            var defaultValue = default(T);

            var value = this.FirstOrDefault(v => v.ColumnName.Equals(column.Name, COMPARISON));

            if (value == null)
            {
                return defaultValue;
            }

            var o = value.Value;

            if (o is T)
                return (T)o;

            return defaultValue;
        }
    }
}