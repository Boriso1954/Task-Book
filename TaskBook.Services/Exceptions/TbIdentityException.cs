using System;
using Microsoft.AspNet.Identity;

namespace TaskBook.Services.Interfaces
{
    /// <summary>
    /// TaskBook identity exception
    /// </summary>
    public sealed class TbIdentityException: Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TbIdentityException()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public TbIdentityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Reference to the source exception</param>
        public TbIdentityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="identityResult">Result of the identity operation</param>
        public TbIdentityException(string message, IdentityResult identityResult)
            : base(message)
        {
            TbIdentityResult = identityResult;
        }

        /// <summary>
        /// Accessor to the identity operation
        /// </summary>
        public IdentityResult TbIdentityResult { get; private set; }
    }
}
