using System;
using System.Collections.Generic;
using TaskBook.DataAccessLayer.AuthManagers;

namespace TaskBook.Services.Interfaces
{
    /// <summary>
    /// Defines the RoleService operations
    /// </summary>
    public interface IRoleService: IDisposable
    {
        /// <summary>
        /// Accessor to the UserManager object
        /// </summary>
        TbUserManager UserManager { get; set; }

        /// <summary>
        /// Returns user's roles by user ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User's roles</returns>
        IList<string> GetRolesByUserId(string id);

        /// <summary>
        /// Returns user's roles by user name
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User's roles</returns>
        IList<string> GetRolesByUserName(string userName);
    }
}
