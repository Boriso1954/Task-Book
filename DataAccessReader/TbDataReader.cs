using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessReader.Exceptions;

namespace TaskBook.DataAccessReader
{
    public sealed class TbDataReader: IDisposable
    {
        private SqlConnection _sqlConnection = null;

        public string ConnectionString
        {
            get
            {
                //return ConfigurationManager.ConnectionStrings["TaskBookDbContext"].ConnectionString;
                return ConfigurationManager.ConnectionStrings["TaskBookDbContextTest"].ConnectionString; // Only for test goal
            }
        }

        public void OpenConnection()
        {
            try
            {
                if(_sqlConnection.State != ConnectionState.Open)
                {
                    _sqlConnection.Open();
                }
            }
            catch(SqlException)
            {
                throw;
            }
        }

        // TODO Make it async
        public SqlDataReader ExecuteReader(CommandType commandType, string commandText, TbParameters parameters = null)
        {
            try
            {
                _sqlConnection = new SqlConnection(ConnectionString);
                var sqlCommand = _sqlConnection.CreateCommand();
                sqlCommand.CommandType = commandType;
                sqlCommand.CommandText = commandText;

                if(parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters.ToArray());
                }
                OpenConnection();
                SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch(SqlException ex)
            {
               
                throw new DataAccessReaderException("SQL error", ex);
            }
            catch(ConfigurationErrorsException ex)
            {
                Dispose();
                throw new DataAccessReaderException("Connection configuration error", ex);
            }
            catch(InvalidCastException ex)
            {
                Dispose();
                throw new DataAccessReaderException("Invalid cast", ex);
            }
            catch(ObjectDisposedException ex)
            {
                Dispose();
                throw new DataAccessReaderException("Object disposed", ex);
            }
            catch(InvalidOperationException ex)
            {
                Dispose();
                throw new DataAccessReaderException("Invalid Operation", ex);
            }
            catch(Exception ex)
            {
                Dispose();
                throw new DataAccessReaderException("Exception", ex);
            }
        }

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
