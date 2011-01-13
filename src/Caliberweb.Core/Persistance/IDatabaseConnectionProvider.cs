namespace Caliberweb.Core.Persistance
{
    public interface IDatabaseConnectionProvider
    {
        string ConnectionString { get; }

        string Default { get; }
    }
}