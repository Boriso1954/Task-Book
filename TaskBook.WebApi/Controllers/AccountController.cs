using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DataAccessLayer.Repositories;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DataAccessReader.ViewModels;
using TaskBook.DomainModel;
using TaskBook.WebApi.Models;

namespace TaskBook.WebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly UserManager<TbUser> _userManager;
        private readonly IProjectUsersRepository _projectUsersRepository;

        public AccountController(IUserStore<TbUser> userStore, 
            IProjectUsersRepository projectUsersRepository)
        {
            _userManager = new UserManager<TbUser>(userStore);
            _projectUsersRepository = projectUsersRepository;
        }

        // GET api/Account/GetUser/{userName}
        [Route("GetUser/{userName}")]
        [ResponseType(typeof(TbUser))]
        public IHttpActionResult GetUser(string userName)
        {
            var user = _userManager.FindByName(userName);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET api/Account/GetUserRolesByUserName/{userName}
        [Route("GetUserRolesByUserName/{userName}")]
        [ResponseType(typeof(IQueryable<string>))]
        public IHttpActionResult GetUserRolesByUserName(string userName)
        {
            var user = _userManager.FindByName(userName);
            if(user == null)
            {
                return NotFound();
            }

            var rolesForUser = _userManager.GetRoles(user.Id);
            if(rolesForUser == null)
            {
                return BadRequest(string.Format("The role for user {} is not found.", userName));
            }
            return Ok(rolesForUser);
        }

        // GET api/Account/GetUserRolesByUserId/{id}
        [Route("GetUserRolesByUserId/{id}")]
        [ResponseType(typeof(IQueryable<string>))]
        public IHttpActionResult GetUserRolesByUserId(string id)
        {
            var rolesForUser = _userManager.GetRoles(id);
            
            if(rolesForUser == null)
            {
                return BadRequest(string.Format("The role for user ID {0} has not been found.", id));
            }
            return Ok(rolesForUser);
        }

        // POST api/Account/AddUser
        [Route("AddUser")]
        public IHttpActionResult AddUser(TbUserVm userModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: introduce mapping
            var user = new TbUser
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
                    var errorResult = GetErrorResult(result);
                    if(errorResult != null)
                    {
                        return errorResult;
                    }

                    string role = userModel.Role;
                    long projectId = (long)userModel.ProjectId;
                    string userId = user.Id;
                    var rolesForUser = _userManager.GetRoles(userId);
                    if(!rolesForUser.Contains(role))
                    {
                        result = _userManager.AddToRole(userId, role);
                        errorResult = GetErrorResult(result);
                        if(errorResult != null)
                        {
                            return errorResult;
                        }
                    }

                    var projectUsers = new ProjectUsers()
                    {
                        ProjectId = projectId,
                        UserId = userId
                    };
                    _projectUsersRepository.Add(projectUsers);
                    _projectUsersRepository.SaveChanges();

                    var notAssignedUser = _userManager.FindByName("NotAssigned");
                    _projectUsersRepository.DeleteByPredicate(x => x.UserId == notAssignedUser.Id && x.ProjectId == projectId);
                    _projectUsersRepository.SaveChanges();
                }
                catch(DataAccessLayerException ex)
                {
                    return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
                }
                transaction.Complete();
            }
            return Ok();
        }

        // PUT api/Account/UpdateUser/{id}
        [Route("UpdateUser/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateUser(string id, TbUserVm userVm)
        {
            if(id != userVm.UserId)
            {
                return BadRequest("User ID conflict.");
            }

            var user = await _userManager.FindByIdAsync(userVm.UserId);
            if(user == null)
            {
                return BadRequest(string.Format("The user {0} is not found.", userVm.UserName));
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            // TODO consider mapping
            user.UserName = userVm.UserName;
            user.Email = userVm.Email;
            user.FirstName = userVm.FirstName;
            user.LastName = userVm.LastName;

            var result = await _userManager.UpdateAsync(user);

            var errorResult = GetErrorResult(result);
            if(errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if(result == null)
            {
                return InternalServerError();
            }

            if(!result.Succeeded)
            {
                if(result.Errors != null)
                {
                    foreach(string error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                return BadRequest(ModelState);
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            _userManager.Dispose();
            _projectUsersRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
