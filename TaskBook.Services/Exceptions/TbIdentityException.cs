using System;
using Microsoft.AspNet.Identity;

namespace TaskBook.Services.Interfaces
{
    public class TbIdentityException: Exception
    {
        public TbIdentityException()
            : base()
        {
        }

        public TbIdentityException(string message)
            : base(message)
        {
        }

        public TbIdentityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public TbIdentityException(string message, IdentityResult identityResult)
            : base(message)
        {
            TbIdentityResult = identityResult;
        }

        public IdentityResult TbIdentityResult { get; private set; }
    }
}
