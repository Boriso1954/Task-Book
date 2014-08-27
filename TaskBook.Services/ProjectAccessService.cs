using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    public sealed class ProjectAccessService: IProjectAccessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectAccessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        } 
 
        public void AddUserToProject(long projectId, string userId)
        {
            var projectUsers = new ProjectUsers()
            {
                ProjectId = projectId,
                UserId = userId
            };

            var projectUsersRepository = _unitOfWork.ProjectUsersRepository;
            projectUsersRepository.Add(projectUsers);
            _unitOfWork.Commit();
        }

        public void RemoveUserFromRoject(long projectId, string userId)
        {
            var projectUsersRepository = _unitOfWork.ProjectUsersRepository;
            projectUsersRepository.DeleteByPredicate(x => x.UserId == userId && x.ProjectId == projectId);
            _unitOfWork.Commit();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
