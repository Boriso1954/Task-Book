using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer
{
    public class AuthDbContext: IdentityDbContext<TbUser>
    {
        public AuthDbContext()
            : base("TaskBookDbContext")
        {
        }

        public static AuthDbContext Create()
        {
            return new AuthDbContext();
        }
    }
}
