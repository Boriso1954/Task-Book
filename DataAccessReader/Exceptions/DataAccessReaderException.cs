using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessReader.Exceptions
{
    public class DataAccessReaderException: Exception
    {
        public DataAccessReaderException()
            : base()
        {
        }

        public DataAccessReaderException(string message)
            : base(message)
        {
        }

        public DataAccessReaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
