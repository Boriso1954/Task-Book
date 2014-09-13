using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DataAccessLayer.Repositories;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;
using TaskBook.DataAccessLayer;
using TaskBook.DomainModel.Mapping;

namespace TaskBook.Services
{
    public sealed class TaskService: ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public TaskVm GetTask(long id)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var task = readerRepository.GetTask(id).FirstOrDefault();
            return task;
        }

        public IQueryable<TaskVm> GetTasks(long? projectId = null)
        {
            IQueryable<TaskVm> tasks;
            var readerRepository = _unitOfWork.ReaderRepository;

            if(projectId == null)
            {
                tasks = readerRepository.GetTasks();
            }
            else
            {
                tasks = readerRepository.GetTasks(projectId);
            }
            return tasks;
        }

        public IQueryable<TaskVm> GetTasksByUserName(string userName)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var tasks = readerRepository.GetTasksByUserName(userName);
            return tasks;
        }

        public void AddTask(TaskVm taskVm)
        {
            var task = _mapper.Map<TaskVm, TbTask>(taskVm);

            using(var readerRepository = _unitOfWork.ReaderRepository)
            {
                task.CreatedById = readerRepository.GetUserByUserName(taskVm.CreatedBy).FirstOrDefault().UserId;
                task.AssignedToId = readerRepository.GetUserByUserName(taskVm.AssignedTo).FirstOrDefault().UserId;
            }

            var taskRepository = _unitOfWork.TaskRepository;
            taskRepository.Add(task);
            _unitOfWork.Commit();
        }

        public void UpdateTask(long id, TaskVm taskVm)
        {
            if(id != taskVm.TaskId)
            {
                throw new Exception("Task ID conflict.");
            }

            var taskRepository = _unitOfWork.TaskRepository;
            var task = taskRepository.GetById(id);
            if(task == null)
            {
                throw new Exception(string.Format("Unable to find task '{0}'.", taskVm.Title));
            }

            task = _mapper.Map<TaskVm, TbTask>(taskVm, task);

            using(var readerRepository = _unitOfWork.ReaderRepository)
            {
                task.AssignedToId = readerRepository.GetUserByUserName(taskVm.AssignedTo).FirstOrDefault().UserId;
            }

            taskRepository.Update(task);
            _unitOfWork.Commit();
        }

        public void DeleteTask(long id)
        {
            var taskRepository = _unitOfWork.TaskRepository;
            var existing = taskRepository.GetById(id);
            if(existing == null)
            {
                throw new Exception(string.Format("Unable to find task to be deleted. task ID {0}", id));
            }

            taskRepository.Delete(existing);
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
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
