using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using TaskBook.DomainModel;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    public sealed class RoleService: IRoleService
    {
        private readonly UserManager<TbUser> _userManager;

        public RoleService(IUserStore<TbUser> userStore)
        {
            _userManager = new UserManager<TbUser>(userStore);
        }

        public IList<string> GetRolesByUserId(string id)
        {
            var rolesForUser = _userManager.GetRoles(id);
            return rolesForUser;
        }

        public IList<string> GetRolesByUserName(string userName)
        {
            var user = _userManager.FindByName(userName);
            if(user == null)
            {
                throw new Exception(string.Format("Unable to find user '{0}'", userName));
            }

            var rolesForUser = _userManager.GetRoles(user.Id);
            return rolesForUser;
        }

        public void Dispose()
        {
            _userManager.Dispose();
        }
    }
}
