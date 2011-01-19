namespace Caliberweb.Core.IO
{
    public interface ICsvValue<T> : ICsvValue
    {
        new T Value { get;  }
    }

    public interface ICsvValue
    {
        string ColumnName { get; }
        object Value { get;  }
    }
}