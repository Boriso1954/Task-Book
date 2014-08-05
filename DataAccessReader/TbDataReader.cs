using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessReader
{
    public sealed class TbDataReader: IDisposable
    {
        private SqlConnection _sqlConnection = null;

        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["TaskBookDbContextTest"].ToString();
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
            catch(SqlException)
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                if(_sqlConnection.State != ConnectionState.Closed)
                {
                    _sqlConnection.Close();
                }
            }
            catch(SqlException)
            {
                throw;
            }
        }
    }
}
