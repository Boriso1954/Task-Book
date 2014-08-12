using System;

namespace TaskBook.Services.Interfaces
{
    public interface IProjectAccessService: IDisposable
    {
        void AddUserToProject(long projectId, string userId);
        void RemoveUserFromRoject(long projectId, string userId);
    }
}
