using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.AuthManagers;

namespace TaskBook.Services.Interfaces
{
    public interface IUserService: IDisposable
    {
        string Host { get; set; }
        TbUserManager UserManager { get; set; }
        TbUser GetById(string id);
        TbUser GetByName(string name);
        Task<TbUser> GetByNameAsync(string userName);
        TbUserRoleVm GetUserByUserName(string userName);
        IQueryable<TbUserRoleVm> GetUsersWithRolesByProjectId(long projectId);
        IQueryable<UserProjectVm> GetUsersByProjectId(long projectId);
        Task ForgotPassword(ForgotPasswordVm model);
        Task ResetPassword(ResetPasswordVm model);
        //void AddUser(TbUserRoleVm userModel);
        Task AddUserAsync(TbUserRoleVm userModel);
        void UpdateUser(string id, TbUserRoleVm userVm);
        //Task UpdateUserAsync(string id, TbUserVm userVm);
        void DeleteUser(string id, bool softDelete = false);
        
    }
}
