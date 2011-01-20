using System;
using System.Collections.Generic;
using System.Linq;

namespace Caliberweb.Core.IO.Csv
{
    public class CsvDescription
    {
        public IEnumerable<IColumn> Columns { get; private set; }

        public CsvDescription(IEnumerable<IColumn> columns)
        {
            Columns = columns;
        }

        public static CsvDescription Empty = new CsvDescription(new IColumn[0]);

        internal IEnumerable<IndexedColumn> FindColumns(string[] headerColumns)
        {
            const StringComparison COMPARISON = StringComparison.InvariantCultureIgnoreCase;
            
            var columns = Columns;

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