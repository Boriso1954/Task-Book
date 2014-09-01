using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Permissions")]
    public class PermissionsController : ApiController
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [Route("GetByRole/{roleName}")]
        [ResponseType(typeof(IQueryable<PermissionVm>))]
        public IHttpActionResult GetByRole(string roleName)
        {
            try
            {
                var permissions = _permissionService.GetByRole(roleName);

                if(permissions == null)
                {
                    return BadRequest(string.Format("Unable to return permissions for role '{0}'", roleName));
                }
                return Ok(permissions);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        protected override void Dispose(bool disposing)
        {
            _permissionService.Dispose();
            base.Dispose(disposing);
        }

    }
}
