using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    public sealed class ProjectAccessService: IProjectAccessService
    {
        private readonly IProjectUsersRepository _projectUsersRepository;

        public ProjectAccessService(IProjectUsersRepository projectUsersRepository)
        {
            _projectUsersRepository = projectUsersRepository;
        } 
 
        public void AddUserToProject(long projectId, string userId)
        {
            var projectUsers = new ProjectUsers()
            {
                ProjectId = projectId,
                UserId = userId
            };
            _projectUsersRepository.Add(projectUsers);
            _projectUsersRepository.SaveChanges();
        }

        public void RemoveUserFromRoject(long projectId, string userId)
        {
            _projectUsersRepository.DeleteByPredicate(x => x.UserId == userId && x.ProjectId == projectId);
            _projectUsersRepository.SaveChanges();
        }

        public void Dispose()
        {
            _projectUsersRepository.Dispose();
        }
    }
}
