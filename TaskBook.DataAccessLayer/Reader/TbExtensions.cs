using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessLayer.Reader
{
    internal static class TbExtensions
    {
        internal static IEnumerable<T> Select<T>(this SqlDataReader reader, Func<SqlDataReader, T> projection)
        {
            while(reader.Read())
            {
                yield return projection(reader);
            }
        }
    }
}
