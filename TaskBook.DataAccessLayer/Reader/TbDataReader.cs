using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Exceptions;

namespace TaskBook.DataAccessLayer.Reader
{
    public sealed class TbDataReader: IDisposable
    {
        private readonly TaskBookDbContext _database;
        private readonly SqlConnection _sqlConnection;

        public TbDataReader(TaskBookDbContext database)
        {
            _database = database;
            _sqlConnection = (SqlConnection)_database.Database.Connection;
        }

        // TODO Make it async
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
