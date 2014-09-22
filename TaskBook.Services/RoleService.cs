using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using TaskBook.DomainModel;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    /// <summary>
    /// Service to manage user roles in the database
    /// </summary>
    public sealed class RoleService: IRoleService
    {
        /// <summary>
        /// Constructor; uses by the Unity dependency injector
        /// </summary>
        [InjectionConstructor]
        public RoleService()
        {
        }

        /// <summary>
        /// Accessor to the UserManager object
        /// </summary>
        public TbUserManager UserManager { get; set; }

        /// <summary>
        /// Returns user's roles by user ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User's roles</returns>
        public IList<string> GetRolesByUserId(string id)
        {
            var rolesForUser = UserManager.GetRoles(id);
            return rolesForUser;
        }

        /// <summary>
        /// Returns user's roles by user name
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User's roles</returns>
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
