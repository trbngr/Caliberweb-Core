using System;
using System.Collections.Generic;
using System.Data;

namespace Caliberweb.Core.Persistance
{
    public interface ISqlHelper
    {
        /// <summary>
        /// Executes a statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery(string commandText);

        /// <summary>
        /// Executes a statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery(string commandText, IEnumerable<IDataParameter> parameters);

        /// <summary>
        /// Executes a statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType, IEnumerable<IDataParameter> parameters);

        /// <summary>
        /// Executes a query and returns a single <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create the <see cref="T"/>.</param>
        /// <returns>The <see cref="T"/> instance.</returns>
        T QueryRecord<T>(string commandText, Func<IDataReader, T> read);

        /// <summary>
        /// Executes a query and returns a single <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create the <see cref="T"/>.</param>
        /// <returns>The <see cref="T"/> instance.</returns>
        T QueryRecord<T>(string commandText, IEnumerable<IDataParameter> parameters, Func<IDataReader, T> read);

        /// <summary>
        /// Executes a query and returns a single <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create the <see cref="T"/>.</param>
        /// <returns>The <see cref="T"/> instance.</returns>
        T QueryRecord<T>(string commandText, CommandType commandType, IEnumerable<IDataParameter> parameters, Func<IDataReader, T> read);

        /// <summary>
        /// Executes a query and returns a list of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create an instance of <see cref="T"/>.</param>
        /// <returns>The list of <see cref="T"/> records.</returns>
        IEnumerable<T> QueryRecordSet<T>(string commandText, Func<IDataReader, T> read);

        /// <summary>
        /// Executes a query and returns a list of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create an instance of <see cref="T"/>.</param>
        /// <returns>The list of <see cref="T"/> records.</returns>
        IEnumerable<T> QueryRecordSet<T>(string commandText, IEnumerable<IDataParameter> parameters, Func<IDataReader, T> read);

        /// <summary>
        /// Executes a query and returns a list of <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the record to return.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="read">A delegate that will read the <see cref="IDataReader"/> and create an instance of <see cref="T"/>.</param>
        /// <returns>The list of <see cref="T"/> records.</returns>
        IEnumerable<T> QueryRecordSet<T>(string commandText, CommandType commandType, IEnumerable<IDataParameter> parameters, Func<IDataReader, T> read);

        /// <summary>
        /// Sets the active connection.
        /// </summary>
        /// <param name="selector">The selector.</param>
        void SetConnection(Func<IDatabaseConnectionProvider, string> selector);

        /// <summary>
        /// Gets the database connection provider.
        /// </summary>
        /// <value>The database connection provider.</value>
        IDatabaseConnectionProvider DatabaseConnectionProvider { get; }

        /// <summary>
        /// Sets the default <see cref="System.Data.CommandType"/>.
        /// </summary>
        /// <value>The default <see cref="System.Data.CommandType"/>.</value>
        CommandType DefaultCommandType { set; }

        /// <summary>
        /// Sets the active connection to use and returns an <see cref="IDisposable"/> that will reset it to it's previous value.
        /// </summary>
        /// <example>
        /// using(helper.Connection(p=&gt;p.BillingDb))<br/>
        /// {...}
        /// </example>
        /// <param name="selector">The selector.</param>
        /// <returns>The <see cref="IDisposable"/> object.</returns>
        IDisposable Connection(Func<IDatabaseConnectionProvider, string> selector);

        /// <summary>
        /// Sets the active <see cref="System.Data.CommandType"/> to use and returns an <see cref="IDisposable"/> that will reset it to it's previous value.
        /// </summary>
        /// <example>
        /// using(helper.CommandType(CommandType.Text))<br />
        /// {...}
        /// </example>
        /// <param name="type">The <see cref="System.Data.CommandType"/> to use.</param>
        /// <returns>The <see cref="IDisposable"/> object.</returns>
        IDisposable CommandType(CommandType type);
    }
}