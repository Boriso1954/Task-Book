using System;
using System.Data;
using System.Data.SqlClient;
using TaskBook.DataAccessLayer.Exceptions;

namespace TaskBook.DataAccessLayer.Reader
{
    /// <summary>
    /// Data reader class
    /// </summary>
    public sealed class TbDataReader: IDisposable
    {
        private readonly TaskBookDbContext _dbContext;
        private readonly SqlConnection _sqlConnection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="database">Database context</param>
        public TbDataReader(TaskBookDbContext database)
        {
            _dbContext = database;
            _sqlConnection = (SqlConnection)_dbContext.Database.Connection;
        }

        // TODO: Make async version
        /// <summary>
        /// Executes data reading from the database
        /// </summary>
        /// <param name="commandType">Type of a command to be executed</param>
        /// <param name="commandText">Here is a name of stored procedure</param>
        /// <param name="parameters">Command parameters</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(CommandType commandType, string commandText, TbParameters parameters = null)
        {
            try
            {
                var sqlCommand = _sqlConnection.CreateCommand();
                sqlCommand.CommandType = commandType;
                sqlCommand.CommandText = commandText;

                if(parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters.ToArray());
                }

                if(_sqlConnection.State != ConnectionState.Open)
                {
                   _sqlConnection.Open();
                }
                var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch(SqlException ex)
            {
                throw new DataAccessReaderException("SQL error", ex);
            }
            catch(InvalidCastException ex)
            {
                throw new DataAccessReaderException("Invalid cast", ex);
            }
            catch(ObjectDisposedException ex)
            {
                throw new DataAccessReaderException("Object disposed", ex);
            }
            catch(InvalidOperationException ex)
            {
                throw new DataAccessReaderException("Invalid Operation", ex);
            }
            catch(Exception ex)
            {
                throw new DataAccessReaderException("Exception", ex);
            }
        }

        /// <summary>
        /// Closes connection to the database
        /// </summary>
        public void Dispose()
        {
            if(_sqlConnection == null)
            {
                return;
            }

            try
            {
                if(_sqlConnection.State != ConnectionState.Closed)
                {
                    _sqlConnection.Close();
                }
            }
            catch(SqlException ex)
            {
                throw new DataAccessReaderException("SQL error", ex);
            }
        }
    }
}
