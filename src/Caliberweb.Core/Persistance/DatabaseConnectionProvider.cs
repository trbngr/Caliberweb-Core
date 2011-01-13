namespace Caliberweb.Core.Persistance
{
    internal class DatabaseConnectionProvider : IDatabaseConnectionProvider
    {
        public DatabaseConnectionProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #region IDatabaseConnectionProvider Members

        public string ConnectionString { get; private set; }

        public string Default
        {
            get { return ConnectionString; }
        }

        #endregion
    }
}