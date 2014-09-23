using System;

namespace TaskBook.Services.Interfaces
{
    /// <summary>
    /// Defines the ProjectAccessService operations
    /// </summary>
    public interface IProjectAccessService: IDisposable
    {
        /// <summary>
        /// Adds user to the project; a user should be registered in the system beforehand
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="userId">User ID</param>
        void AddUserToProject(long projectId, string userId);

        /// <summary>
        /// Deletes a user from the project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="userId">User ID</param>
        void RemoveUserFromRoject(long projectId, string userId);
    }
}
