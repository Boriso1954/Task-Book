﻿using System;
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
using TaskBook.WebApi.Attributes;
using System.Web.Http.Tracing;

namespace TaskBook.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Tasks")]
    public class TasksController : ApiController
    {
        private readonly ITaskService _taskService;
        private readonly ITraceWriter _logger;

        public TasksController(ITaskService taskService,
            ITraceWriter logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // GET api/Tasks/GetTasks
        [Route("GetTasks")]
        [ResponseType(typeof(IQueryable<TaskVm>))]
        [AuthorizeRoles(RoleKey.Admin)] // This is for special cases
        public IHttpActionResult GetTasks()
        {
            try
            {
                var tasks = _taskService.GetTasks();
                if(tasks == null)
                {
                    string msg = "Unable to return tasks";
                    _logger.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, msg);
                    return BadRequest(msg);
                }
                return Ok(tasks);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
        }

        // GET api/Tasks/GetTasks/{projectId}
        [Route("GetTasks/{projectId:long}")]
        [ResponseType(typeof(IQueryable<TaskVm>))]
        [AuthorizeRoles(RoleKey.Manager, RoleKey.AdvancedUser)]
        public IHttpActionResult GetTasks(long projectId)
        {
            try
            {
                var tasks = _taskService.GetTasks(projectId);
                if(tasks == null)
                {
                    string msg = string.Format("Unable to return tasks for project with ID = {0}", projectId);
                    _logger.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, msg);
                    return BadRequest(msg);
                }
                return Ok(tasks);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
        }

        // GET api/Tasks/GetTask/{id}
        [Route("GetTask/{id:long}")]
        [ResponseType(typeof(TaskVm))]
        [AuthorizeRoles(RoleKey.Manager, RoleKey.AdvancedUser, RoleKey.User)]
        public IHttpActionResult GetTask(long id)
        {
            try
            {
                var task = _taskService.GetTask(id);
                if(task == null)
                {
                    string msg = string.Format("Unable to return the task with ID = {0}", id);
                    _logger.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, msg);
                    return BadRequest(msg);
                }
                return Ok(task);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
        }

        // GET api/Tasks/GetTasks/{userName}
        [Route("GetTasks/{userName}")]
        [ResponseType(typeof(TaskVm))]
        [AuthorizeRoles(RoleKey.User)]
        public IHttpActionResult GetTasks(string userName)
        {
            try
            {
                var tasks = _taskService.GetTasksByUserName(userName);
                if(tasks == null)
                {
                    string msg = string.Format("Unable to return tasks for user '{0}'", userName);
                    _logger.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, msg);
                    return BadRequest(msg);
                }
                return Ok(tasks);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
        }

        // PUT api/Tasks/AddTask
        [Route("AddTask")]
        [HttpPost]
        [AuthorizeRoles(RoleKey.Manager, RoleKey.AdvancedUser)]
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
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // PUT api/Tasks/UpdateTask/{id}
        [Route("UpdateTask/{id:long}")]
        [HttpPut]
        [AuthorizeRoles(RoleKey.Manager, RoleKey.AdvancedUser, RoleKey.User)]
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
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
            catch(DataAccessReaderException ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // DELETE api/Tasks/DeleteTask/{id}
        [Route("DeleteTask/{id:long}")]
        [HttpDelete]
        [AuthorizeRoles(RoleKey.Manager, RoleKey.AdvancedUser)]
        public IHttpActionResult DeleteTask(long id)
        {
            try
            {
                _taskService.DeleteTask(id);
            }
            catch(DataAccessLayerException ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, ex);
                return BadRequest(ex.Message);
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