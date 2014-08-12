using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.Services.Interfaces;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.WebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        // GET api/Account/GetUserByUserName/{userName}
        [Route("GetUserByUserName/{userName}")]
        [ResponseType(typeof(TbUserVm))]
        public IHttpActionResult GetUserByUserName(string userName)
        {
            var userVm = _userService.GetUserByUserName(userName);
            if(userVm == null)
            {
                return BadRequest(string.Format("User '{0}' has not been found.", userName));
            }
            return Ok(userVm);
        }

        // POST api/Account/AddUser
        [Route("AddUser")]
        public IHttpActionResult AddUser(TbUserVm userModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userService.AddUser(userModel);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(TbIdentityException ex)
            {
                return GetErrorResult(ex.TbIdentityResult);
            }
            return Ok();
        }

        // PUT api/Account/UpdateUser/{id}
        [Route("UpdateUser/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateUser(string id, TbUserVm userVm)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.UpdateUserAsync(id, userVm);
            }
            catch(TbIdentityException ex)
            {
                return GetErrorResult(ex.TbIdentityResult);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if(result == null)
            {
                return InternalServerError();
            }
            else // !result.Succeeded
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
        }

        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();
            base.Dispose(disposing);
        }
    }
}
