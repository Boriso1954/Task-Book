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

namespace TaskBook.Services
{
    public sealed class UserService: IUserService
    {
        private readonly UserManager<TbUser> _userManager;
        private readonly IProjectAccessService _projectAccessService;
        private readonly ReaderRepository _readerRepository;

        public UserService(IUserStore<TbUser> userStore,
            IProjectAccessService projectAccessService,
            ReaderRepository readerRepository)
        {
            _userManager = new UserManager<TbUser>(userStore);
            _projectAccessService = projectAccessService;
            _readerRepository = readerRepository;
        }

        public TbUser GetById(string id)
        {
            var user = _userManager.FindById(id);
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

            using(var transaction = new TransactionScope())
            {
                try
                {
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

                    _projectAccessService.AddUserToProject(projectId, userId);

                    string notAssignedUserId = _userManager.FindByName("NotAssigned").Id;
                    _projectAccessService.RemoveUserFromRoject(projectId, notAssignedUserId);
                }
                catch(DataAccessLayerException)
                {
                    throw;
                }
                catch(TbIdentityException)
                {
                    throw;
                }
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

            using(var transaction = new TransactionScope())
            {
                try
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
                }
                catch(DataAccessLayerException)
                {
                    throw;
                }
                catch(TbIdentityException)
                {
                    throw;
                }
                transaction.Complete();
            }
        }

        public void Dispose()
        {
            _userManager.Dispose();
            _projectAccessService.Dispose();
        }
    }
}
