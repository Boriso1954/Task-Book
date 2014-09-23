using System;
using System.Linq;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.Services.Interfaces
{
    /// <summary>
    /// Defines the UserService operations
    /// </summary>
    public interface IUserService: IDisposable
    {
        /// <summary>
        /// Accessor to the host URL
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Accessor to the UserManager object
        /// </summary>
        TbUserManager UserManager { get; set; }

        /// <summary>
        /// Returns user's data including role by user name
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User's data including role</returns>
        TbUserRoleVm GetUserWithRoleByUserName(string userName);

        /// <summary>
        /// Returns users with their roles for the project
        /// </summary>
        /// <param name="projectId"Project ID></param>
        /// <returns>Users with their roles</returns>
        IQueryable<TbUserRoleVm> GetUsersWithRolesByProjectId(long projectId);

        /// <summary>
        /// Returns users short data for the project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Users short data</returns>
        IQueryable<UserProjectVm> GetUsersByProjectId(long projectId);

        /// <summary>
        /// Creates and send email message to a user to reset a password
        /// </summary>
        /// <param name="model">User's data (email)</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task ForgotPassword(ForgotPasswordVm model);

        /// <summary>
        /// Resets a password
        /// </summary>
        /// <param name="model">User's data to reset password</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task ResetPassword(ResetPasswordVm model);

        /// <summary>
        /// Adds a user to the system asyncronously
        /// </summary>
        /// <param name="userModel">User's data</param>
        /// <returns>Task to enable asynchronous execution</returns>
        Task AddUserAsync(TbUserRoleVm userModel);

        /// <summary>
        /// Updates user's data in the database
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="userVm">Input user's data</param>
        void UpdateUser(string id, TbUserRoleVm userVm);

        /// <summary>
        /// Removes a user from the database 
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="softDeleted">TRUE if the project should be only marked as deleted</param>
        /// <remarks>During user deletion all the user's tasks must be deleted as well</remarks>
        void DeleteUser(string id, bool softDelete = false);
        
    }
}
