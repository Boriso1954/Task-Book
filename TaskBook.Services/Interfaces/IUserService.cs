using System;
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
        TbUserVm GetUserByUserName(string userName);
        IQueryable<TbUserVm> GetUsersWithRolesByProjectId(long projectId);
        IQueryable<UserProjectVm> GetUsersByProjectId(long projectId);
        void AddUser(TbUserVm userModel);
        void UpdateUser(string id, TbUserVm userVm);
        //Task UpdateUserAsync(string id, TbUserVm userVm);
        void DeleteUser(string id);
    }
}
