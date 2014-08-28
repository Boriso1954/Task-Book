using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.Services.Interfaces
{
    public interface IUserService: IDisposable
    {
        TbUser GetById(string id);
        TbUser GetByName(string name);
        TbUserRoleVm GetUserByUserName(string userName);
        IQueryable<TbUserRoleVm> GetUsersWithRolesByProjectId(long projectId);
        IEnumerable<UserProjectVm> GetUsersByProjectId(long projectId);
        void AddUser(TbUserRoleVm userModel);
        void UpdateUser(string id, TbUserRoleVm userVm);
        //Task UpdateUserAsync(string id, TbUserVm userVm);
        void DeleteUser(string id, bool softDelete = false);
        
    }
}
