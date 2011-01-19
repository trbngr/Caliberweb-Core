using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Caliberweb.Core.IO.Csv
{
    public class CsvValueList : IEnumerable<ICsvValue>
    {
        private readonly IEnumerable<ICsvValue> collection;
        private const StringComparison COMPARISON = StringComparison.InvariantCultureIgnoreCase;

        public CsvValueList(IEnumerable<ICsvValue> collection)
        {
            this.collection = collection;
        }

        public T GetValue<T>(IColumn<T> column)
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

        public IEnumerator<ICsvValue> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}