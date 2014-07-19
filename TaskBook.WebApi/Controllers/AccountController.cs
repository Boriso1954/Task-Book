using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TaskBook.DataAccessLayer.Repositories;
using TaskBook.DomainModel;
using TaskBook.WebApi.Models;

namespace TaskBook.WebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST api/Account/Register
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterUserViewModel userModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email
            };

            IdentityResult result = await _userRepository.RegisterUser(user, userModel.Password);
            IHttpActionResult errorResult = GetErrorResult(result);
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
            if(disposing)
            {
                _userRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
