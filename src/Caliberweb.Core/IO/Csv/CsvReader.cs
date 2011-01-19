using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Caliberweb.Core.Extensions;

namespace Caliberweb.Core.IO.Csv
{
    public class CsvReader : IComparable<CsvReader>
    {
        private readonly FileInfo file;
        private readonly CsvDescription description;
        private static readonly Func<FileInfo, string[]> lineReader;

        static CsvReader()
        {
            //memoize this function to minimize IO
            lineReader = new Func<FileInfo, string[]>(f => File.ReadAllLines(f.FullName)).Memoize();
        }

        public CsvReader(FileInfo file, CsvDescription description)
        {
            file.Refresh();
            if (!file.Exists)
            {
                throw new FileNotFoundException("not found", file.FullName);
            }

            this.file = file;
            this.description = description;
        }

        public IEnumerable<ICsvRecord> GetRecords()
        {
            var queue = new Queue<string>(lineReader(file));

            var columns = FindColumns(queue);

            var records = queue.Select(line => CreateRecord(line, columns)).ToList();

            return records;
        }

        private static ICsvRecord CreateRecord(string line, IEnumerable<IndexedColumn> columns)
        {
            var parts = line.Split('\t');

            var values = columns.Select(column =>
            {
                var value = parts[column.Ordinal].Replace("\"", "");
                return column.GetValue(value);
            });

            return new CsvRecord(values);
        }

        private IEnumerable<IndexedColumn> FindColumns(Queue<string> queue)
        {
            var columns = queue.Dequeue().Split('\t');

            return description.FindColumns(columns).ToArray();
        }

        public int CompareTo(CsvReader other)
        {
            return file.FullName.CompareTo(other.file.FullName);
        }
    }
}