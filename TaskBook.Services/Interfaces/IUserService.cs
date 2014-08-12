using System;
using System.Threading.Tasks;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.Services.Interfaces
{
    public interface IUserService: IDisposable
    {
        TbUserVm GetUserByUserName(string userName);
        void AddUser(TbUserVm userModel);
        //void UpdateUser(string id, TbUserVm userVm);
        Task UpdateUserAsync(string id, TbUserVm userVm);
    }
}
