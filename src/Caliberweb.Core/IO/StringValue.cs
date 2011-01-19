namespace Caliberweb.Core.IO
{
    class StringValue : CsvValue<string>
    {
        public StringValue(string columnName, string stringValue) : base(columnName, stringValue)
        {}

        public override string Value
        {
            get { return stringValue; }
        }
    }
}