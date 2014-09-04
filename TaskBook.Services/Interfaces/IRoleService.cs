using System;
using System.Collections.Generic;
using TaskBook.Services.AuthManagers;

namespace TaskBook.Services.Interfaces
{
    public interface IRoleService: IDisposable
    {
        TbUserManager UserManager { get; set; }
        IList<string> GetRolesByUserId(string id);
        IList<string> GetRolesByUserName(string userName);
    }
}
