using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessReader;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    public sealed class PermissionService: IPermissionService
    {
        private readonly ReaderRepository _readerRepository;

        public PermissionService(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public IQueryable<PermissionVm> GetByRole(string roleName)
        {
            var permissions = _readerRepository.GetPermissionsByRole(roleName);
            return permissions;
        }

        public void Dispose()
        {
            _readerRepository.Dispose();
        }

    }
}
