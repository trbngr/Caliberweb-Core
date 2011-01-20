using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Caliberweb.Core.Extensions;

using OpenFileSystem.IO;

namespace Caliberweb.Core.IO.Csv
{
    public class CsvReader : IEquatable<CsvReader>
    {
        private readonly IFile file;
        private readonly CsvDescription description;
        private static readonly Func<IFile, string[]> lineReader;

        static CsvReader()
        {
            //memoize this function to minimize IO
            lineReader = new Func<IFile, string[]>(f => f.ReadAllLines()).Memoize();
        }

        public CsvReader(IFile file, CsvDescription description)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException("not found", file.Path);
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

        public bool Equals(CsvReader other)
        {
            return file.Path.Equals(other.file.Path);
        }
    }
}