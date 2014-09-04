using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;
using TaskBook.Services.AuthManagers;
using TaskBook.Services.Interfaces;

namespace TaskBook.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Roles")]
    public class RolesController : ApiController
    {
        private readonly IRoleService _roleService;
        private TbUserManager _userManager;

        [InjectionConstructor]
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
            _roleService.UserManager = UserManager;
            
        }

        public RolesController(IRoleService roleService,
            TbUserManager userManager)
        {
            _roleService = roleService;
            UserManager = userManager;
        }

        public TbUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<TbUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET api/Roles/GetRolesByUserName/{userName}
        [Route("GetRolesByUserName/{userName}")]
        [ResponseType(typeof(IQueryable<string>))]
        public IHttpActionResult GetRolesByUserName(string userName)
        {
            try
            {
                var roles = _roleService.GetRolesByUserName(userName);
                if(roles == null)
                {
                    return BadRequest(string.Format("Unable to find role for user '{0}'.", userName));
                }
                return Ok(roles);
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        // GET api/Account/GetRolesByUserId/{id}
        [Route("GetRolesByUserId/{id}")]
        [ResponseType(typeof(IQueryable<string>))]
        public IHttpActionResult GetRolesByUserId(string id)
        {
            try
            {
                var roles = _roleService.GetRolesByUserId(id);
                if(roles == null)
                {
                    return BadRequest(string.Format("Unable to find role for user with ID '{0}'.", id));
                }
                return Ok(roles);
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        protected override void Dispose(bool disposing)
        {
            _roleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
