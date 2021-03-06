﻿using System;

namespace TaskBook.DataAccessLayer.Exceptions
{
    /// <summary>
    /// Specific exception for the data reader
    /// </summary>
    public sealed class DataAccessReaderException: Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataAccessReaderException()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public DataAccessReaderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error messge</param>
        /// <param name="innerException">Reference to the source exception</param>
        public DataAccessReaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
