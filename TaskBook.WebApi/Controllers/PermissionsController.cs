using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using NLog.Mvc;
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
        private readonly ILogger _logger;

        public PermissionsController(IPermissionService permissionService,
            ILogger logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        // GET api//Permissions/GetByRole/{roleName}
        [Route("GetByRole/{roleName}")]
        [ResponseType(typeof(IQueryable<PermissionVm>))]
        public IHttpActionResult GetByRole(string roleName)
        {
            try
            {
                var permissions = _permissionService.GetByRole(roleName);

                if(permissions == null)
                {
                    string msg = string.Format("Unable to return permissions for role '{0}'", roleName);
                    _logger.Warning(msg);
                    return BadRequest(msg);
                }
                return Ok(permissions);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _permissionService.Dispose();
            base.Dispose(disposing);
        }

    }
}
