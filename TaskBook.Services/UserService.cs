using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DomainModel;
using TaskBook.DomainModel.Mapping;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    /// <summary>
    /// Service to manage TaskBook users in the database
    /// </summary>
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

        /// <summary>
        /// Constructor; uses by the Unity dependency injector
        /// </summary>
        /// <param name="unitOfWork">Represents UnitOfWork object</param>
        /// <param name="emailService">Represents EamilService object</param>
        /// <param name="mapper">Represents AutoMapper object</param>
        [InjectionConstructor]
        public UserService(IUnitOfWork unitOfWork,
            IEmailService emailService,
            IMapper mapper)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Represents UnitOfWork object</param>
        /// <param name="userManager">Represents Identity UserManager object</param>
        /// <param name="emailService">Represents EamilService object</param>
        /// <param name="mapper">Represents AutoMapper object</param>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Represents UnitOfWork object</param>
        /// <param name="userManager">Represents Identity UserManager object</param>
        public UserService(IUnitOfWork unitOfWork,
            TbUserManager userManager)
            : this(unitOfWork, userManager, null, null)
        {
        }

        /// <summary>
        /// Accessor to the host URL
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Accessor to the UserManager object
        /// </summary>
        public TbUserManager UserManager { get; set; }

        /// <summary>
        /// Returns user's data including role by user name
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User's data including role</returns>
        public TbUserRoleVm GetUserWithRoleByUserName(string userName)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var user = readerRepository.GetUserByUserName(userName).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Returns users with their roles for the project
        /// </summary>
        /// <param name="projectId"Project ID></param>
        /// <returns>Users with their roles</returns>
        public IQueryable<TbUserRoleVm> GetUsersWithRolesByProjectId(long projectId)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var users = readerRepository.GetUsersWithRolesByProjectId(projectId);
            return users;
        }

        /// <summary>
        /// Returns users short data for the project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Users short data</returns>
        public IQueryable<UserProjectVm> GetUsersByProjectId(long projectId)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var users = readerRepository.GetUsersByProjectId(projectId);
            return users;
        }

        /// <summary>
        /// Creates and send email message to a user to reset a password
        /// </summary>
        /// <param name="model">User's data (email)</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public async Task ForgotPassword(ForgotPasswordVm model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                throw new TbIdentityException("Find user error");
            }

            // Create a password reset token and email body
            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //code = WebUtility.UrlEncode(code);
            string link = string.Format("{0}/#/resetPassword/{1}/{2}", Host, user.UserName, code);
            string body = string.Format(_resetPswEmailBodyConst, user.FirstName, link);

            // Create the email message to reset a password and send it
            MailMessage message = new MailMessage();
            message.To.Add(user.Email);
            message.Subject = "Reset password";
            message.Body = body;

            await _emailService.SendMailAsync(message);
        }

        /// <summary>
        /// Resets a password
        /// </summary>
        /// <param name="model">User's data to reset password</param>
        /// <returns>Task to enable asynchronous execution</returns>
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

        /// <summary>
        /// Adds a user to the system asyncronously
        /// </summary>
        /// <param name="userModel">User's data</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public async Task AddUserAsync(TbUserRoleVm userModel)
        {
            // Base user constructor creates Id, which must be kept in the model before mapping
            var user = new TbUser();
            userModel.UserId = user.Id;

            // Execute mapping from the view model to the domain object
            user = _mapper.Map<TbUserRoleVm, TbUser>(userModel, user);

            // TODO: consider more secure psw generation 
            string password = "user12";

            // Envelop the sequence of the db operations in the transaction scope
            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                // Create a user
                var result = UserManager.Create(user, password);
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Create user error", result);
                }

                // Add a user to the specified role
                string role = userModel.Role;
                long projectId = (long)userModel.ProjectId;
                string userId = user.Id;

                result = UserManager.AddToRole(userId, role);
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Add to role error", result);
                }

                // Add a user to the specified project
                var projectUsers = new ProjectUsers()
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                var projectUsersRepository = _unitOfWork.ProjectUsersRepository;
                projectUsersRepository.Add(projectUsers);

                // If just added user is a manager, delete "NotAssigned" (manager) system account from the project
                if(role == RoleKey.Manager)
                {
                    string notAssignedUserId = UserManager.FindByName("NotAssigned").Id;
                    projectUsersRepository.DeleteByPredicate(x => x.UserId == notAssignedUserId && x.ProjectId == projectId);
                }
                _unitOfWork.Commit();
                transaction.Complete();
            }

            // Create email notification and send it
            string login = string.Format("{0}/#/login", Host);
            string retrive = string.Format("{0}/#/forgotPassword", Host);
            string body = string.Format(_addUserEmailBodyConst, user.FirstName, user.UserName, password, retrive, login);
            MailMessage message = new MailMessage();
            message.To.Add(user.Email);
            message.Subject = "Add account";
            message.Body = body;

            await _emailService.SendMailAsync(message);
        }

        /// <summary>
        /// Updates user's data in the database
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="userVm">Input user's data</param>
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

            // Execute mapping from the view model to the domain object
            user = _mapper.Map<TbUserRoleVm, TbUser>(userVm, user);

            // Envelop the sequence of the db operations in the transaction scope
            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                // Update user's data
                var result = UserManager.Update(user);
                if(result == null || !result.Succeeded)
                {
                    throw new TbIdentityException("Update user error", result);
                }
            
                // Change user's role if it had been changed
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

        /// <summary>
        /// Removes a user from the database 
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="softDeleted">TRUE if the project should be only marked sa deleted</param>
        /// <remarks>During user deletion all the user's tasks must be deleted as well</remarks>
        public void DeleteUser(string id, bool softDeleted = false)
        {
            var user = UserManager.FindById(id);
            if(user == null)
            {
                throw new Exception(string.Format("Unable to delete user with ID '{0}'.", id));
            }

            // Envelop the sequence of the db operations in the transaction scope
            using(var transaction = TransactionProvider.GetTransactionScope())
            {
                // Delete user's tasks first
                IList<TaskUserVm> tasks;
                using(var readerRepository = _unitOfWork.ReaderRepository)
                {
                    
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

                // Mark User as deleted (soft deletion), but skip the system user
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
            if (_emailService != null)
            {
                _emailService.Dispose();
            }
            _unitOfWork.Dispose();
        }
    }
}
