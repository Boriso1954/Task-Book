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
using TaskBook.DataAccessLayer.Exceptions;

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
            try
            {
                var tasks = _taskService.GetTasks();
                if(tasks == null)
                {
                    return BadRequest("Unable to return tasks");
                }
                return Ok(tasks);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        // GET api/Tasks/GetTasks/{projectId}
        [Route("GetTasks/{projectId:long}")]
        [ResponseType(typeof(IQueryable<TaskVm>))]
        public IHttpActionResult GetTasks(long projectId)
        {
            try
            {
                var tasks = _taskService.GetTasks(projectId);
                if(tasks == null)
                {
                    return BadRequest("Unable to return tasks");
                }
                return Ok(tasks);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        // GET api/Tasks/GetTask/{id}
        [Route("GetTask/{id:long}")]
        [ResponseType(typeof(TaskVm))]
        public IHttpActionResult GetTask(long id)
        {
            try
            {
                var task = _taskService.GetTask(id);
                if(task == null)
                {
                    return BadRequest(string.Format("Unable to return the task with id = {0}", id));
                }
                return Ok(task);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        // GET api/Tasks/GetTasks/{userName}
        [Route("GetTasks/{userName}")]
        [ResponseType(typeof(TaskVm))]
        public IHttpActionResult GetTasks(string userName)
        {
            try
            {
                var tasks = _taskService.GetTasksByUserName(userName);
                if(tasks == null)
                {
                    return BadRequest("Unable to return tasks");
                }
                return Ok(tasks);
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
        }

        // PUT api/Tasks/AddTask
        [Route("AddTask")]
        [HttpPost]
        public IHttpActionResult AddTask(TaskVm taskVm)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _taskService.AddTask(taskVm);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }

            return Ok();
        }

        // PUT api/Tasks/UpdateTask/{id}
        [Route("UpdateTask/{id:long}")]
        [HttpPut]
        public IHttpActionResult UpdateTask(long id, TaskVm taskVm)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _taskService.UpdateTask(id, taskVm);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(DataAccessReaderException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }

            return Ok();
        }

        // DELETE api/Tasks/DeleteTask/{id}
        [Route("DeleteTask/{id:long}")]
        [HttpDelete]
        public IHttpActionResult DeleteTask(long id)
        {
            try
            {
                _taskService.DeleteTask(id);
            }
            catch(DataAccessLayerException ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }
            catch(Exception ex)
            {
                return BadRequest(string.Format("{0}: {1}", ex.Message, ex.InnerException.Message));
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            _taskService.Dispose();
            base.Dispose(disposing);
        }
    }
}