using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using TaskBook.DomainModel;
using TaskBook.Services.AuthManagers;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    public sealed class RoleService: IRoleService
    {

        [InjectionConstructor]
        public RoleService()
        {
        }

        public RoleService(TbUserManager userManager)
        {
            UserManager = userManager;
        }

        public TbUserManager UserManager { get; set; }

        public IList<string> GetRolesByUserId(string id)
        {
            var rolesForUser = UserManager.GetRoles(id);
            return rolesForUser;
        }

        public IList<string> GetRolesByUserName(string userName)
        {
            var user = UserManager.FindByName(userName);
            if(user == null)
            {
                throw new Exception(string.Format("Unable to find user '{0}'", userName));
            }

            var rolesForUser = UserManager.GetRoles(user.Id);
            return rolesForUser;
        }

        public void Dispose()
        {
            UserManager.Dispose();
        }
    }
}
