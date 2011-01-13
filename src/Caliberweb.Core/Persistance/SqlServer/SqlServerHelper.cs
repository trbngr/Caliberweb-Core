using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Caliberweb.Core.Persistance.SqlServer
{
    public class SqlServerHelper : ISqlHelper
    {
        private readonly IDatabaseConnectionProvider provider;
        private string connectionString;

        public SqlServerHelper(IDatabaseConnectionProvider provider)
        {
            this.provider = provider;
            DefaultCommandType = System.Data.CommandType.Text;
        }

        #region ISqlHelper Members

        /// <summary>
        /// Executes a statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, DefaultCommandType, new IDataParameter[0]);
        }

        /// <summary>
        /// Executes a statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string commandText, IEnumerable<IDataParameter> parameters)
        {
            return ExecuteNonQuery(commandText, DefaultCommandType, parameters);
        }

        /// <summary>
        /// Executes a statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType,
                                    IEnumerable<IDataParameter> parameters)
        {
            EnsureConnectionIsSet();

            using (var context = new SqlExecutionContext(connectionString, commandText, commandType, parameters))
            {
                return context.Execute();
            }
        }

        /// <summary>
        /// Executes a query and returns a single <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create the <see cref="T"/>.</param>
        /// <returns>The <see cref="T"/> instance.</returns>
        public T QueryRecord<T>(string commandText, Func<IDataReader, T> read)
        {
            return QueryRecord(commandText, DefaultCommandType, new IDataParameter[0], read);
        }

        /// <summary>
        /// Executes a query and returns a single <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create the <see cref="T"/>.</param>
        /// <returns>The <see cref="T"/> instance.</returns>
        public T QueryRecord<T>(string commandText, IEnumerable<IDataParameter> parameters, Func<IDataReader, T> read)
        {
            return QueryRecord(commandText, DefaultCommandType, parameters, read);
        }

        /// <summary>
        /// Executes a query and returns a single <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create the <see cref="T"/>.</param>
        /// <returns>The <see cref="T"/> instance.</returns>
        public T QueryRecord<T>(string commandText, CommandType commandType, IEnumerable<IDataParameter> parameters,
                                Func<IDataReader, T> read)
        {
            EnsureConnectionIsSet();
            T result = default(T);

            using (var context = new SqlExecutionContext(connectionString, commandText, commandType, parameters))
            {
                context.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        result = read(reader);
                    }
                });
            }

            return result;
        }

        /// <summary>
        /// Executes a query and returns a list of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create an instance of <see cref="T"/>.</param>
        /// <returns>The list of <see cref="T"/> records.</returns>
        public IEnumerable<T> QueryRecordSet<T>(string commandText, Func<IDataReader, T> read)
        {
            return QueryRecordSet(commandText, DefaultCommandType, new IDataParameter[0], read);
        }

        /// <summary>
        /// Executes a query and returns a list of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create an instance of <see cref="T"/>.</param>
        /// <returns>The list of <see cref="T"/> records.</returns>
        public IEnumerable<T> QueryRecordSet<T>(string commandText, IEnumerable<IDataParameter> parameters,
                                                Func<IDataReader, T> read)
        {
            return QueryRecordSet(commandText, DefaultCommandType, parameters, read);
        }

        /// <summary>
        /// Executes a query and returns a list of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create an instance of <see cref="T"/>.</param>
        /// <returns>The list of <see cref="T"/> records.</returns>
        public IEnumerable<T> QueryRecordSet<T>(string commandText, CommandType commandType,
                                                IEnumerable<IDataParameter> parameters,
                                                Func<IDataReader, T> read)
        {
            EnsureConnectionIsSet();

            using (var context = new SqlExecutionContext(connectionString, commandText, commandType, parameters))
            {
                var result = new HashSet<T>();

                context.Execute(reader =>
                {
                    while (reader.Read())
                    {
                        result.Add(read(reader));
                    }
                });

                return result;
            }
        }

        /// <summary>
        /// Sets the active connection.
        /// </summary>
        /// <param name="selector">The selector.</param>
        public void SetConnection(Func<IDatabaseConnectionProvider, string> selector)
        {
            connectionString = selector(provider);
        }

        /// <summary>
        /// Gets the database connection provider.
        /// </summary>
        /// <value>The database connection provider.</value>
        public IDatabaseConnectionProvider DatabaseConnectionProvider
        {
            get { return provider; }
        }

        /// <summary>
        /// Sets the default <see cref="System.Data.CommandType"/>.
        /// </summary>
        /// <value>The default <see cref="System.Data.CommandType"/>.</value>
        public CommandType DefaultCommandType { private get; set; }

        /// <summary>
        /// Sets the active connection to use and returns an <see cref="IDisposable"/> that will reset it to it's previous value.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns>The <see cref="IDisposable"/> object.</returns>
        /// <example>
        /// using(helper.Connection(p=&gt;p.BillingDb))<br/>
        /// {...}
        /// </example>
        public IDisposable Connection(Func<IDatabaseConnectionProvider, string> selector)
        {
            string currentConnection = connectionString;

            connectionString = selector(provider);

            return new DisposableAction(() => connectionString = currentConnection);
        }

        /// <summary>
        /// Sets the active <see cref="System.Data.CommandType"/> to use and returns an <see cref="IDisposable"/> that will reset it to it's previous value.
        /// </summary>
        /// <param name="type">The <see cref="System.Data.CommandType"/> to use.</param>
        /// <returns>The <see cref="IDisposable"/> object.</returns>
        /// <example>
        /// using(helper.CommandType(CommandType.Text))<br/>
        /// {...}
        /// </example>
        public IDisposable CommandType(CommandType type)
        {
            CommandType currentType = DefaultCommandType;

            DefaultCommandType = type;

            return new DisposableAction(() => DefaultCommandType = currentType);
        }

        #endregion

        private void EnsureConnectionIsSet()
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = provider.Default;
            }
        }

        internal class SqlExecutionContext : IDisposable
        {
            private readonly string commandText;
            private readonly CommandType commandType;
            private readonly string connectionString;
            private readonly IEnumerable<IDataParameter> parameters;
            private readonly Stack<IDisposable> disposables;

            public SqlExecutionContext(string connectionString, string commandText, CommandType commandType,
                                       IEnumerable<IDataParameter> parameters)
            {
                this.connectionString = connectionString;
                this.commandText = commandText;
                this.commandType = commandType;
                this.parameters = parameters;

                disposables = new Stack<IDisposable>();
            }

            public SqlExecutionContext(string connectionString)
                : this(connectionString, null, System.Data.CommandType.Text, null)
            {
            }

            #region IDisposable Members

            public void Dispose()
            {
                while (disposables.Count > 0)
                {
                    Dispose(disposables.Pop());
                }
            }

            private static void Dispose(IDisposable disposable)
            {
                if (disposable == null)
                    return;

                disposable.Dispose();
            }

            #endregion

            public void Execute(Action<IDataReader> context)
            {
                var connection = new SqlConnection(connectionString);
                SqlCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = commandType;

                AddParameters(command, parameters);

                connection.Open();

                IDataReader reader = command.ExecuteReader();

                RegisterDisposables(reader, command, connection);

                context(reader);

                connection.Close();
            }

            public int Execute()
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                RegisterDisposables(transaction, connection);

                int recordsAffected;

                try
                {
                    var command = new SqlCommand(commandText, connection, transaction)
                    {
                        CommandType = commandType
                    };

                    AddParameters(command, parameters);

                    RegisterDisposables(command);

                    recordsAffected = command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }

                return recordsAffected;
            }

            private void RegisterDisposables(params IDisposable[] list)
            {
                foreach (var disposable in list)
                {
                    disposables.Push(disposable);
                }
            }

            private static void AddParameters(IDbCommand command, IEnumerable<IDataParameter> parameters)
            {
                if (parameters == null)
                {
                    return;
                }

                foreach (IDataParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
        }

    }
}