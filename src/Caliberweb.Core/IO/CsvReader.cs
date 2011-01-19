using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Caliberweb.Core.IO
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
            var header = queue.Dequeue();

            var columns = header.Split('\t');

            return description.FindColumns(columns).ToArray();
        }
    }

    internal class IndexedColumn : IColumn
    {
        private readonly IColumn wrapped;

        public IndexedColumn(IColumn wrapped)
        {
            this.wrapped = wrapped;
        }

        public int Ordinal { get; set; }

        public string Name
        {
            get { return wrapped.Name; }
        }

        public ICsvValue GetValue(string value)
        {
            return wrapped.GetValue(value);
        }
    }

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

    public interface IColumn
    {
        string Name { get; }
        ICsvValue GetValue(string value);
    }
    
    public interface IColumn<T> : IColumn
    {
        new ICsvValue<T> GetValue(string value);
    }

    public static class Columns
    {
        public static IColumn<string> String(string name)
        {
            return new StringColumn(name);
        }

        public static IColumn<DateTime> Date(string name)
        {
            return new DateColumn(name);
        }

        public static IColumn<int> Integer(string name)
        {
            return new IntColumn(name);
        }

        private abstract class Column<T> : IColumn<T>
        {
            protected Column(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }
            public abstract ICsvValue<T> GetValue(string value);
            ICsvValue IColumn.GetValue(string value)
            {
                return GetValue(value);
            }
        }

        private class IntColumn : Column<int>
        {
            public IntColumn(string name) : base(name)
            {}

            public override ICsvValue<int> GetValue(string value)
            {
                return new IntValue(Name, value);
            }
        }

        private class DateColumn : Column<DateTime>
        {
            public DateColumn(string name) : base(name)
            {}

            public override ICsvValue<DateTime> GetValue(string value)
            {
                return new DateValue(Name, value);
            }
        }

        private class StringColumn : Column<string>
        {
            public StringColumn(string name) : base(name)
            {}

            public override ICsvValue<string> GetValue(string value)
            {
                return new StringValue(Name, value);
            }
        }
    }

}