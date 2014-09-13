using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TaskBook.DataAccessLayer;
using TaskBook.DomainModel;
using TaskBook.Services;

namespace TaskBook.Services.AuthManagers
{
    public sealed class TbUserManager: UserManager<TbUser>
    {
        public TbUserManager(IUserStore<TbUser> userStore)
            : base(userStore)
        {
        }

        public static TbUserManager Create(IdentityFactoryOptions<TbUserManager> options, IOwinContext context)
        {
            var userManager = new TbUserManager(new UserStore<TbUser>(context.Get<TaskBookDbContext>()));

            // http://tech.trailmax.info/2014/06/asp-net-identity-and-cryptographicexception-when-running-your-site-on-microsoft-azure-web-sites/
            userManager.UserTokenProvider = new EmailTokenProvider<TbUser, string>();
            //var dataProtectionProvider = options.DataProtectionProvider;
            //if(dataProtectionProvider != null)
            //{
            //    userManager.UserTokenProvider = new DataProtectorTokenProvider<TbUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}

            return userManager;
        }
    }
}
