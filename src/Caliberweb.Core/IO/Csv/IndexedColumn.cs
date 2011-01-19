namespace Caliberweb.Core.IO.Csv
{
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
}