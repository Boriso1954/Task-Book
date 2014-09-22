using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    /// <summary>
    /// Service to manage ProjectUsers entities in the database
    /// </summary>
    public sealed class ProjectAccessService: IProjectAccessService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Represents UnitOfWork object</param>
        public ProjectAccessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        } 
 
        /// <summary>
        /// Adds user to the project; a user should be registered in the system beforehand
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="userId">User ID</param>
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

        /// <summary>
        /// Deletes a user from the project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="userId">User ID</param>
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
