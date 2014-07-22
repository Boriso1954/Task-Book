using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.AuthManagers
{
    public class TbUserManager: UserManager<TbUser>
    {
        public TbUserManager(IUserStore<TbUser> userStore)
            : base(userStore)
        {
        }

        public static TbUserManager Create(IdentityFactoryOptions<TbUserManager> options, IOwinContext context)
        {
            var userManager = new TbUserManager(new UserStore<TbUser>(context.Get<TaskBookDbContext>()));
            return userManager;
        }
    }
}
