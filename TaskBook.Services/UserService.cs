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
using System.Net.Mail;
using Microsoft.AspNet.Identity.Owin;
using TaskBook.Services.AuthManagers;
using System.Net;
using TaskBook.DomainModel.Mapping;

namespace TaskBook.Services
{
    public sealed class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        private const string _addUserEmailBodyConst = @"
                <div>
	                Hello {0}!
                    <p>
	                    Your account has been added successfully to the TaskBook system. Your credentials:
                        <br/>User name: <strong>{1}</strong>
                        <br/>Password: <strong>{2}</strong>
                        <br/>Please change your initial password by clicking this <a href={3}>link</a> (recommended).
                        <br/>Please click <a href={4}>log in</a> to enter to the system,
                    </p>
                    TaskBook system
                </div>
                ";

        private const string _resetPswEmailBodyConst = @"
                <div>
	                Hello {0}!
                    <p>
                        Please reset your password by clicking here: <a href={1}>Reset</a>
                    </p>
                    TaskBook system
                </div>
                ";

        [InjectionConstructor]
        public UserService(IUnitOfWork unitOfWork,
            IEmailService emailService,
            IMapper mapper)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public UserService(IUnitOfWork unitOfWork,
            TbUserManager userManager,
            IEmailService emailService,
            IMapper mapper)
        {
            _emailService = emailService;
            UserManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public UserService(IUnitOfWork unitOfWork,
            TbUserManager userManager)
            : this(unitOfWork, userManager, null, null)
        {
        }

        public string Host { get; set; }
        public TbUserManager UserManager { get; set; }

        public TbUser GetById(string id)
        {
            var user = UserManager.FindById(id);
            return user;
            
        }

        public TbUser GetByName(string name)
        {
            var user = UserManager.FindByName(name);
            return user;
        }

        public Task<TbUser> GetByNameAsync(string name)
        {
           return UserManager.FindByNameAsync(name);
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

        public IQueryable<UserProjectVm> GetUsersByProjectId(long projectId)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var users = readerRepository.GetUsersByProjectId(projectId);
            return users;
        }

        public async Task ForgotPassword(ForgotPasswordVm model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                throw new TbIdentityException("Find user error");
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //code = WebUtility.UrlEncode(code);
            string link = string.Format("{0}/#/resetPassword/{1}/{2}", Host, user.UserName, code);
            //string link = string.Format("{0}/#/resetPassword?userId={1}&code={2}", Host, user.UserName, code);
            string body = string.Format(_resetPswEmailBodyConst, user.FirstName, link);
            MailMessage message = new MailMessage();
            message.To.Add(user.Email);
            message.Subject = "Reset password";
            message.Body = body;

            await _emailService.SendMailAsync(message);
        }

        public async Task ResetPassword(ResetPasswordVm model)
        {
            var user = await UserManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                throw new TbIdentityException("Find user error.");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if(result == null || !result.Succeeded)
            {
                throw new TbIdentityException("Reset password error.", result);
            }
        }

        public async Task AddUserAsync(TbUserRoleVm userModel)
        {
            // Base user constructor creates Id, which must be kept in the model before mapping
            var user = new TbUser();
            userModel.UserId = user.Id;
            user = _mapper.Map<TbUserRoleVm, TbUser>(userModel, user);

            string password = "user12";
            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                var result = UserManager.Create(user, password);
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Create user error", result);
                }

                string role = userModel.Role;
                long projectId = (long)userModel.ProjectId;
                string userId = user.Id;

                result = UserManager.AddToRole(userId, role);
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
                    string notAssignedUserId = UserManager.FindByName("NotAssigned").Id;
                    projectUsersRepository.DeleteByPredicate(x => x.UserId == notAssignedUserId && x.ProjectId == projectId);
                }
                _unitOfWork.Commit();
                transaction.Complete();
            }

            // Send email notification
            string login = string.Format("{0}/#/login", Host);
            string retrive = string.Format("{0}/#/forgotPassword", Host);
            string body = string.Format(_addUserEmailBodyConst, user.FirstName, user.UserName, password, retrive, login);
            MailMessage message = new MailMessage();
            message.To.Add(user.Email);
            message.Subject = "Add account";
            message.Body = body;

            await _emailService.SendMailAsync(message);
        }

        public void UpdateUser(string id, TbUserRoleVm userVm)
        {
            if(id != userVm.UserId)
            {
                throw new Exception("User ID conflict.");
            }

            var user = UserManager.FindById(userVm.UserId);
            if(user == null)
            {
                throw new Exception(string.Format("Unable to find user '{0}'.", userVm.UserName));
            }

            user = _mapper.Map<TbUserRoleVm, TbUser>(userVm, user);

            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                var result = UserManager.Update(user);
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Update user error", result);
                }
            
                string prevRole = UserManager.GetRoles(id).FirstOrDefault();
                if(prevRole != userVm.Role)
                {
                    result = UserManager.RemoveFromRole(id, prevRole);
                    if(result == null || !result.Succeeded)
                    {
                        throw new TbIdentityException("Remove from role error", result);
                    }
                    result = UserManager.AddToRole(id, userVm.Role);
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
            var user = UserManager.FindById(id);
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
                    var result = UserManager.Update(user);
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
                    var deletedUser = UserManager.FindById(u.UserId);

                    // Delete user (cascade deletion from UserRoles and ProgectUsers)
                    UserManager.Delete(deletedUser);
                }
                
            }
        }
        
        public void Dispose()
        {
            UserManager.Dispose();
            _emailService.Dispose();
            _unitOfWork.Dispose();
        }
    }
}
