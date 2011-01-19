namespace Caliberweb.Core.IO
{
    class IntValue : CsvValue<int>
    {
        public IntValue(string columnName, string stringValue) : base(columnName, stringValue)
        {}

        public override int Value
        {
            get { return int.Parse(stringValue); }
        }
    }
}