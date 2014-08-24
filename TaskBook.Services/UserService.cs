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
using TaskBook.DataAccessReader;
using TaskBook.DataAccessLayer.Repositories.Interfaces;

namespace TaskBook.Services
{
    public sealed class UserService: IUserService
    {
        private readonly UserManager<TbUser> _userManager;
        private readonly IProjectAccessService _projectAccessService;
        private readonly ITaskRepository _taskRepository;
        private readonly ReaderRepository _readerRepository;

        public UserService(IUserStore<TbUser> userStore,
            IProjectAccessService projectAccessService,
            ITaskRepository taskRepository,
            ReaderRepository readerRepository)
        {
            _userManager = new UserManager<TbUser>(userStore);
            _projectAccessService = projectAccessService;
            _taskRepository = taskRepository;
            _readerRepository = readerRepository;
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


        public TbUserVm GetUserByUserName(string userName)
        {
            var user = _readerRepository.GetUserByUserName(userName).FirstOrDefault();
            return user;
        }

        public IQueryable<TbUserVm> GetUsersWithRolesByProjectId(long projectId)
        {
            var users = _readerRepository.GetUsersWithRolesByProjectId(projectId);
            return users;
        }

        public IQueryable<UserProjectVm> GetUsersByProjectId(long projectId)
        {
            var users = _readerRepository.GetUsersByProjectId(projectId);
            return users;
        }

        public void AddUser(TbUserVm userModel)
        {
            // TODO: introduce mapping
            var user = new TbUser()
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email
            };
            
            var result = _userManager.Create(user, "123456");
            if(result == null || !result.Succeeded)
            {
                throw new TbIdentityException("Create user error", result);
            }

            string role = userModel.Role;
            long projectId = (long)userModel.ProjectId;
            string userId = user.Id;
            var rolesForUser = _userManager.GetRoles(userId);
            
            if(!rolesForUser.Contains(role))
            {
                result = _userManager.AddToRole(userId, role);
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Add to role error", result);
                }
            }

            using(var transaction = new TransactionScope())
            {
                _projectAccessService.AddUserToProject(projectId, userId);

                string notAssignedUserId = _userManager.FindByName("NotAssigned").Id;
                _projectAccessService.RemoveUserFromRoject(projectId, notAssignedUserId);
               
                transaction.Complete();
            }
        }

        public void UpdateUser(string id, TbUserVm userVm)
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
            
            var result = _userManager.Update(user);
            if(result == null || !result.Succeeded)
            {
                throw new TbIdentityException("Update user error", result);
            }
            using(var transaction = new TransactionScope())
            {
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

        public void DeleteUser(string id)
        {
            var user = _userManager.FindById(id);
            if(user == null)
            {
                throw new Exception(string.Format("Unable to delete user with ID '{0}'.", id));
            }

            user.DeletedDate = DateTimeOffset.UtcNow;

            var result = _userManager.Update(user);
            if(result == null || !result.Succeeded)
            {
                throw new TbIdentityException("Delete user error", result);
            }

            var tasks = _readerRepository.GetUserTasksByUserName(user.UserName);
            using(var transaction = new TransactionScope())
            {
                foreach(var t in tasks)
                {
                    var task = _taskRepository.GetById(t.TaskId);
                    _taskRepository.Delete(task);
                }
                _taskRepository.SaveChanges();
                transaction.Complete();
            }
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _projectAccessService.Dispose();
            _taskRepository.Dispose();
            _readerRepository.Dispose();
        }
    }
}
