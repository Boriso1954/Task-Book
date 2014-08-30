using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNet.Identity;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DomainModel;
using TaskBook.Services.Interfaces;
using TaskBook.DomainModel.ViewModels;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DataAccessLayer;
using Microsoft.Practices.Unity;

namespace TaskBook.Services
{
    public sealed class UserService: IUserService
    {
        private readonly UserManager<TbUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        [InjectionConstructor]
        public UserService(IUnitOfWork unitOfWork, IUserStore<TbUser> userStore)
        {
            _userManager = new UserManager<TbUser>(userStore);
            _unitOfWork = unitOfWork;
        }

        public UserService(IUnitOfWork unitOfWork, UserManager<TbUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public TbUser GetById(string id)
        {
            var user = _userManager.FindById(id);
            return user;
        }

        public TbUser GetByName(string name)
        {
            var user = _userManager.FindByName(name);
            return user;
        }

        public TbUserRoleVm GetUserByUserName(string userName)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var user = readerRepository.GetUserByUserName(userName).FirstOrDefault();
            return user;
        }

        public IQueryable<TbUserRoleVm> GetUsersWithRolesByProjectId(long projectId)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var users = readerRepository.GetUsersWithRolesByProjectId(projectId);
            return users;
        }

        public IEnumerable<UserProjectVm> GetUsersByProjectId(long projectId)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var users = readerRepository.GetUsersByProjectId(projectId);
            return users;
        }

        public void AddUser(TbUserRoleVm userModel)
        {
            // TODO: introduce mapping
            var user = new TbUser()
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email
            };
            
            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                var result = _userManager.Create(user, "user12");
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Create user error", result);
                }

                string role = userModel.Role;
                long projectId = (long)userModel.ProjectId;
                string userId = user.Id;

                result = _userManager.AddToRole(userId, role);
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Add to role error", result);
                }

                var projectUsers = new ProjectUsers()
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                var projectUsersRepository = _unitOfWork.ProjectUsersRepository;
                projectUsersRepository.Add(projectUsers);

                if(role == RoleKey.Manager)
                {
                    string notAssignedUserId = _userManager.FindByName("NotAssigned").Id;
                    projectUsersRepository.DeleteByPredicate(x => x.UserId == notAssignedUserId && x.ProjectId == projectId);
                }
                _unitOfWork.Commit();
                transaction.Complete();
            }
        }

        public void UpdateUser(string id, TbUserRoleVm userVm)
        {
            if(id != userVm.UserId)
            {
                throw new Exception("User ID conflict.");
            }

            var user = _userManager.FindById(userVm.UserId);
            if(user == null)
            {
                throw new Exception(string.Format("Unable to find user '{0}'.", userVm.UserName));
            }

            // TODO consider mapping
            user.UserName = userVm.UserName;
            user.Email = userVm.Email;
            user.FirstName = userVm.FirstName;
            user.LastName = userVm.LastName;

            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                var result = _userManager.Update(user);
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Update user error", result);
                }
            
                string prevRole = _userManager.GetRoles(id).FirstOrDefault();
                if(prevRole != userVm.Role)
                {
                    result = _userManager.RemoveFromRole(id, prevRole);
                    if(result == null || !result.Succeeded)
                    {
                        throw new TbIdentityException("Remove from role error", result);
                    }
                    result = _userManager.AddToRole(id, userVm.Role);
                    if(result == null || !result.Succeeded)
                    {
                        throw new TbIdentityException("Add to role error", result);
                    }
                }
                transaction.Complete();
            }
        }

        public void DeleteUser(string id, bool softDeleted = false)
        {
            var user = _userManager.FindById(id);
            if(user == null)
            {
                throw new Exception(string.Format("Unable to delete user with ID '{0}'.", id));
            }

            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                IList<TaskUserVm> tasks;
                using(var readerRepository = _unitOfWork.ReaderRepository)
                {
                    // Delete user's tasks first
                    tasks = readerRepository.GetUserTasks(user.UserName).ToList();
                }

                if(tasks.Any())
                {
                    var taskRepository = _unitOfWork.TaskRepository;
                    foreach(var t in tasks)
                    {
                        var task = taskRepository.GetById(t.TaskId);
                        taskRepository.Delete(task);
                    }
                    _unitOfWork.Commit();
                }

                // Mark User as deleted (soft deletion), skip the system user
                if(user.UserName != "NotAssigned")
                {
                    user.DeletedDate = DateTimeOffset.UtcNow;
                    var result = _userManager.Update(user);
                    if(result == null || !result.Succeeded)
                    {
                        throw new TbIdentityException("Update user error", result);
                    }
                }
                transaction.Complete();
            }

            if(!softDeleted)
            {
                // Delete users which marked as deleted 
                IList<TbUserVm> deletedUsers;
                using(var readerRepository = _unitOfWork.ReaderRepository)
                {
                   deletedUsers = readerRepository.GetDeletedUsers().ToList();
                }
                
                foreach(var u in deletedUsers)
                {
                    var deletedUser = _userManager.FindById(u.UserId);

                    // Delete user (cascade deletion from UserRoles and ProgectUsers)
                    _userManager.Delete(deletedUser);
                }
                
            }
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _unitOfWork.Dispose();
        }
    }
}
