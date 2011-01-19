using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Caliberweb.Core.IO.Csv
{
    public class CsvReader
    {
        private readonly FileInfo file;
        private readonly CsvDescription description;

        public CsvReader(FileInfo file, CsvDescription description)
        {
            file.Refresh();
            if(!file.Exists)
            {
                throw new FileNotFoundException("not found", file.FullName);
            }

            this.file = file;
            this.description = description;
        }

        public IEnumerable<ICsvRecord> GetRecords()
        {
            var lines = File.ReadAllLines(file.FullName);
            
            var queue = new Queue<string>(lines);

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
    }
}