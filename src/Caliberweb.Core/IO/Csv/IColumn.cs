namespace Caliberweb.Core.IO.Csv
{
    public interface IColumn<T> : IColumn
    {
        new ICsvValue<T> GetValue(string value);
    }

    public interface IColumn
    {
        string Name { get; }
        ICsvValue GetValue(string value);
    }
}