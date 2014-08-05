using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DataAccessLayer.Repositories;
using TaskBook.DataAccessReader.ViewModels;
using TaskBook.DomainModel;
using TaskBook.WebApi.Models;

namespace TaskBook.WebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly UserManager<TbUser> _userManager;

        public AccountController(IUserStore<TbUser> userStore)
        {
            _userManager = new UserManager<TbUser>(userStore);
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
            rolesForUser = null;
            if(rolesForUser == null)
            {
                return BadRequest(string.Format("The role for user ID {0} has not been found.", id));
            }
            return Ok(rolesForUser);
        }

        // POST api/Account/Register
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterUserViewModel userModel)
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

            var result = await _userManager.CreateAsync(user, userModel.Password);
            var errorResult = GetErrorResult(result);
            if(errorResult != null)
            {
                return errorResult;
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
            base.Dispose(disposing);
        }
    }
}
