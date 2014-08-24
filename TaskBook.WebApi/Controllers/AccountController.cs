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
using TaskBook.DataAccessReader.Exceptions;

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
            try
            {
                var userVm = _userService.GetUserByUserName(userName);
                if(userVm == null)
                {
                    return BadRequest(string.Format("Unable to return data for user '{0}'.", userName));
                }
                return Ok(userVm);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        // GET api/Account/GetUsersWithRolesByProjectId/{projectId}
        [Route("GetUsersWithRolesByProjectId/{projectId:long}")]
        [ResponseType(typeof(IQueryable<TbUserVm>))]
        public IHttpActionResult GetUsersWithRolesByProjectId(long projectId)
        {
            try
            {
                var users = _userService.GetUsersWithRolesByProjectId(projectId);
                if(users == null)
                {
                    return BadRequest(string.Format("Unable to return users for the project with ID '{0}'.", projectId));
                }
                return Ok(users);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        // GET api/Account/GetUsersByProjectId/{projectId}
        [Route("GetUsersByProjectId/{projectId:long}")]
        [ResponseType(typeof(IQueryable<UserProjectVm>))]
        public IHttpActionResult GetUsersByProjectId(long projectId)
        {
            try
            {
                var users = _userService.GetUsersByProjectId(projectId);
                if(users == null)
                {
                    return BadRequest(string.Format("Unable to return users for the project with ID '{0}'.", projectId));
                }
                return Ok(users);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
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
        public IHttpActionResult UpdateUser(string id, TbUserVm userVm)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userService.UpdateUser(id, userVm);
            }
            catch(TbIdentityException ex)
            {
                return GetErrorResult(ex.TbIdentityResult);
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
           
            return Ok();
        }

        // DELETE api/Account/DeleteUser/{id}
        [Route("DeleteUser/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(string id)
        {
            try
            {
                _userService.DeleteUser(id);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(TbIdentityException ex)
            {
                return GetErrorResult(ex.TbIdentityResult);
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
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
