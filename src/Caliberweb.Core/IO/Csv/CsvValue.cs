using System.Diagnostics;

namespace Caliberweb.Core.IO.Csv
{
    [DebuggerDisplay("{ColumnName}: {Value}")]
    abstract class CsvValue<T> : ICsvValue<T>
    {
        protected readonly string stringValue;

        protected CsvValue(string columnName, string stringValue)
        {
            this.stringValue = stringValue;
            ColumnName = columnName;
        }

        public string ColumnName { get; private set; }

        object ICsvValue.Value
        {
            get { return Value; }
        }

        public abstract T Value { get; }
    }
}