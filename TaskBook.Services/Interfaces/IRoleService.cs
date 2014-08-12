using System;
using System.Collections.Generic;

namespace TaskBook.Services.Interfaces
{
    public interface IRoleService: IDisposable
    {
        IList<string> GetRolesByUserId(string id);
        IList<string> GetRolesByUserName(string userName);
    }
}
