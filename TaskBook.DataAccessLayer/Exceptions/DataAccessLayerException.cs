using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessLayer.Exceptions
{
    public class DataAccessLayerException: Exception
    {
        public DataAccessLayerException()
            : base()
        {
        }

        public DataAccessLayerException(string message)
            : base(message)
        {
        }

        public DataAccessLayerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
