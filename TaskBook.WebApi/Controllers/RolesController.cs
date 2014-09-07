using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Tracing;
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
        private readonly ITraceWriter _logger;

        [InjectionConstructor]
        public RolesController(IRoleService roleService,
            ITraceWriter logger)
        {
            _roleService = roleService;
            _roleService.UserManager = UserManager;
            _logger = logger;
        }

        public RolesController(IRoleService roleService,
            TbUserManager userManager,
            ITraceWriter logger)
        {
            _roleService = roleService;
            UserManager = userManager;
            _logger = logger;
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
                    string msg = string.Format("Unable to find role for user '{0}'.", userName);
                    _logger.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, msg);
                    return BadRequest(msg);
                }
                return Ok(roles);
            }
            catch(Exception ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
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
                    string msg = string.Format("Unable to find role for user with ID '{0}'.", id);
                    _logger.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, msg);
                    return BadRequest(msg);
                }
                return Ok(roles);
            }
            catch(Exception ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _roleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
