using System;

namespace Caliberweb.Core.IO.Csv
{
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