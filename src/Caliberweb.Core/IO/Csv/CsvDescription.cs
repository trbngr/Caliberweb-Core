using System;
using System.Collections.Generic;
using System.Linq;

namespace Caliberweb.Core.IO.Csv
{
    public class CsvDescription
    {
        private const StringComparison COMPARISON_TYPE = StringComparison.InvariantCultureIgnoreCase;
        private const char DEFAULT_SEPARATOR = ',';
        public static CsvDescription Empty = new CsvDescription(new char(), new IColumn[0]);
        private readonly char separator;

        public CsvDescription(IEnumerable<IColumn> columns) : this(DEFAULT_SEPARATOR, columns)
        {}

        public CsvDescription(char separator, IEnumerable<IColumn> columns)
        {
            this.separator = separator;
            Columns = columns;
        }

        public IEnumerable<IColumn> Columns { get; private set; }

        internal IEnumerable<IndexedColumn> FindColumns(string header)
        {
            IEnumerable<IColumn> columns = Columns;

            string[] line = ReadLine(header);

            for (int i = 0; i < line.Length; i++)
            {
                string current = line[i];

                IColumn value = columns.FirstOrDefault(v => v.Name.Equals(current, COMPARISON_TYPE));
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

        internal string[] ReadLine(string line)
        {
            return line.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Replace("\"", ""))
                .ToArray();
        }
    }
}