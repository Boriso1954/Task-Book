using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskBook.DomainModel;
using TaskBook.DataAccessLayer;
using TaskBook.Services.Interfaces;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Tasks")]
    public class TasksController : ApiController
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET api/Tasks/GetTasks
        [Route("GetTasks")]
        [ResponseType(typeof(IQueryable<TaskVm>))]
        public IHttpActionResult GetTasks()
        {
            var tasks = _taskService.GetTasks();
            if(tasks == null)
            {
                return BadRequest("Unable to return tasks");
            }
            return Ok(tasks);
        }

        // GET api/Tasks/GetTasks/{projectId}
        [Route("GetTasks/{projectId:long}")]
        [ResponseType(typeof(IQueryable<TaskVm>))]
        public IHttpActionResult GetTasks(long projectId)
        {
            var tasks = _taskService.GetTasks(projectId);
            if(tasks == null)
            {
                return BadRequest("Unable to return tasks");
            }
            return Ok(tasks);
        }

        protected override void Dispose(bool disposing)
        {
            _taskService.Dispose();
            base.Dispose(disposing);
        }
    }
}