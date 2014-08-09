using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskBook.DataAccessReader;
using TaskBook.DataAccessReader.ViewModels;

namespace TaskBook.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/UserDetails")]
    public class UserDetailsController: ApiController
    {
        private readonly ReaderRepository _readerRepository;

        public UserDetailsController(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        // GET api/UserDetails/GetUserDetailsByUserName/{userName}
        [Route("GetUserDetailsByUserName/{userName}")]
        [ResponseType(typeof(TbUserVm))]
        public IHttpActionResult GetUserDetailsByUserName(string userName)
        {
            var user = _readerRepository.GetUserDetailsByUserName(userName).FirstOrDefault();
            
            if(user == null)
            {
                return BadRequest(string.Format("User '{0}' has not been found.", userName));
            }
            return Ok(user);
        }

        // TODO Obsolete
        // GET api/UserDetails/GetUserPermissionsByUserName
        [Route("GetUserPermissionsByUserName/{userName}")]
        [ResponseType(typeof(IQueryable<PermissionVm>))]
        public IHttpActionResult GetUserPermissionsByUserName(string userName)
        {
            var userPermissions = _readerRepository.GetUserPermissionsByUserName(userName);
            //return BadRequest("Bad request");
            return Ok(userPermissions);
        }

        [Route("GetPermissionsByRole/{roleName}")]
        [ResponseType(typeof(IQueryable<PermissionVm>))]
        public IHttpActionResult GetPermissionsByRole(string roleName)
        {
            var permissions = _readerRepository.GetPermissionsByRole(roleName);
            //return BadRequest("Bad request");
            return Ok(permissions);
        }

        protected override void Dispose(bool disposing)
        {
            _readerRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
