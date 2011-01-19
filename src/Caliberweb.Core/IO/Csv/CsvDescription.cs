using System;
using System.Collections.Generic;
using System.Linq;

namespace Caliberweb.Core.IO.Csv
{
    public class CsvDescription
    {
        private readonly IEnumerable<IColumn> columns;

        public CsvDescription(IEnumerable<IColumn> columns)
        {
            this.columns = columns;
        }

        internal IEnumerable<IndexedColumn> FindColumns(string[] headerColumns)
        {
            const StringComparison COMPARISON = StringComparison.InvariantCultureIgnoreCase;

            for (int i = 0; i < headerColumns.Length; i++)
            {
                var current = headerColumns[i].Replace("\"", "");
                var value = columns.FirstOrDefault(v => v.Name.Equals(current, COMPARISON));
                if (value != null)
                {
                    var indexedColumn = new IndexedColumn(value)
                    {
                        Ordinal = i
                    };

                    yield return indexedColumn;
                }
            }
        }

        
    }
}