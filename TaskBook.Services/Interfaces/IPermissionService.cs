using System;
using System.Linq;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.Services.Interfaces
{
    /// <summary>
    /// Defines the PermissionService operations
    /// </summary>
    public interface IPermissionService: IDisposable
    {
        /// <summary>
        /// Returns permissions for the role
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Role permissions</returns>
        IQueryable<PermissionVm> GetByRole(string roleName);
    }
}
