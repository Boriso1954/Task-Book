using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DataAccessLayer.Repositories;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DataAccessReader;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    public sealed class TaskService: ITaskService
    {
        private readonly ReaderRepository _readerRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserService _userService;

        public TaskService(ReaderRepository readerRepository,
            ITaskRepository taskRepository,
            IUserService userService)
        {
            _readerRepository = readerRepository;
            _taskRepository = taskRepository;
            _userService = userService;
        }

        public TaskVm GetTask(long id)
        {
            var task = _readerRepository.GetTask(id).FirstOrDefault();

            return task;
        }

        public IQueryable<TaskVm> GetTasks(long? projectId = null)
        {
            IQueryable<TaskVm> tasks = null;

            if(projectId == null)
            {
                tasks = _readerRepository.GetTasks();
            }
            else
            {
                tasks = _readerRepository.GetTasks(projectId);
            }
            return tasks;
        }

        public void AddTask(TaskVm taskVm)
        {
            var task = new TbTask()
            {
                Title = taskVm.Title,
                Description = taskVm.Description,
                ProjectId = taskVm.ProjectId,
                CreatedDate = DateTimeOffset.UtcNow,
                CreatedById = _userService.GetUserByUserName(taskVm.CreatedBy).UserId,
                AssignedToId = _userService.GetUserByUserName(taskVm.AssignedTo).UserId,
                DueDate = taskVm.DueDate,
                Status = TbTaskStatus.New
            };

            try
            {
                _taskRepository.Add(task);
                _taskRepository.SaveChanges();
            }
            catch(DataAccessLayerException)
            {
                throw;
            }
        }

        public void UpdateTask(long id, TaskVm taskVm)
        {
            if(id != taskVm.TaskId)
            {
                throw new Exception("Task ID conflict.");
            }

            var task = _taskRepository.GetById(id);

            if(task == null)
            {
                throw new Exception(string.Format("Unable to find task '{0}'.", taskVm.Title));
            }

            // TODO consider mapping
            task.Title = taskVm.Title;
            task.Description = taskVm.Description;
            task.DueDate = taskVm.DueDate;
            task.Status = GetTaskStatusByString(taskVm.Status);
            task.AssignedToId = _userService.GetUserByUserName(taskVm.AssignedTo).UserId;
            task.CompletedDate = taskVm.CompletedDate;

            try
            {
                _taskRepository.Update(task);
                _taskRepository.SaveChanges();
            }
            catch(DataAccessLayerException)
            {
                throw;
            }
        }

        public void DeleteTask(long id)
        {
            var existing = _taskRepository.GetById(id);
            if(existing == null)
            {
                throw new Exception(string.Format("Unable to find task to be deleted. task ID {0}", id));
            }

            try
            {
                _taskRepository.Delete(existing);
                _taskRepository.SaveChanges();
            }
            catch(DataAccessLayerException)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _readerRepository.Dispose();
            _taskRepository.Dispose();
            _userService.Dispose();
        }

        private TbTaskStatus GetTaskStatusByString(string taskStatus)
        {
            TbTaskStatus status = TbTaskStatus.New;
            switch(taskStatus)
            {
                case "New":
                    status = TbTaskStatus.New;
                    break;
                case "In Progress":
                    status = TbTaskStatus.InProgress;
                    break;
                case "Completed":
                    status = TbTaskStatus.Completed;
                    break;
            }
            return status;
        }
    }
}
