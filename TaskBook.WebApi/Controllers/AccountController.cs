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

            IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);
            IHttpActionResult errorResult = GetErrorResult(result);
            if(errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }

        // GET api/Account/GetUserRoles/{userName}
        [Route("GetUserRoles/{userName}")]
        [ResponseType(typeof(IQueryable<string>))]
        public IHttpActionResult GetUserRoles(string userName)
        {
            var user = _userManager.FindByName(userName);
            if(user == null)
            {
                return BadRequest(string.Format("User {} is not found.", userName));
            }

            var rolesForUser = _userManager.GetRoles(user.Id);
            if(rolesForUser == null)
            {
                return BadRequest(string.Format("The role for user {} is not found.", userName));
            }
            return Ok(rolesForUser);
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
