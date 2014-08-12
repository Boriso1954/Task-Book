﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.WebApi.Controllers
{
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
            var permissions = _permissionService.GetByRole(roleName);
            if(permissions == null)
            {
                return BadRequest(string.Format("Unable to return permissions for role '{0}'", roleName));
            }
            return Ok(permissions);
        }

        protected override void Dispose(bool disposing)
        {
            _permissionService.Dispose();
            base.Dispose(disposing);
        }

    }
}
