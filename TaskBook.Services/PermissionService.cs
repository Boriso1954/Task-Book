using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    /// <summary>
    /// Service to manage TaskBook permissions in the database
    /// </summary>
    public sealed class PermissionService: IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Represents UnitOfWork object</param>
        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns permissions for the role
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <returns>Role permissions</returns>
        public IQueryable<PermissionVm> GetByRole(string roleName)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var permissions = readerRepository.GetPermissionsByRole(roleName);
            return permissions;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    }
}
