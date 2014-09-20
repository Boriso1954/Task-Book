using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessLayer.Exceptions
{
    /// <summary>
    /// Data access layer exception. Uses in init of work, etc
    /// </summary>
    public class DataAccessLayerException: Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataAccessLayerException()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public DataAccessLayerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Reference to the source exception</param>
        public DataAccessLayerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
