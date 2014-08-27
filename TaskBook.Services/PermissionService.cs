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
    public sealed class PermissionService: IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
