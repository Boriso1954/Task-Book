using System;
using System.Linq;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;
using TaskBook.DataAccessLayer;
using TaskBook.DomainModel.Mapping;

namespace TaskBook.Services
{
    /// <summary>
    /// Service to manage TaskBook tasks in the database
    /// </summary>
    public sealed class TaskService: ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Represents UnitOfWork object</param>
        /// <param name="mapper">Represents AutoMapper object</param>
        public TaskService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns the task data
        /// </summary>
        /// <param name="id">Ttask ID</param>
        /// <returns>Task data</returns>
        public TaskVm GetTask(long id)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var task = readerRepository.GetTask(id).FirstOrDefault();
            return task;
        }

        /// <summary>
        /// Returns tasks for the project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Tasks for the project</returns>
        /// <remarks>If project id does not specify the method returns tasks for all the projects</remarks>
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

        /// <summary>
        /// Returns user's tasks
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User's tasks</returns>
        public IQueryable<TaskVm> GetTasksByUserName(string userName)
        {
            var readerRepository = _unitOfWork.ReaderRepository;
            var tasks = readerRepository.GetTasksByUserName(userName);
            return tasks;
        }

        /// <summary>
        /// ASdds a task to the database
        /// </summary>
        /// <param name="taskVm">Task data</param>
        public void AddTask(TaskVm taskVm)
        {
            // Execute mapping from the view model to the domain object
            var task = _mapper.Map<TaskVm, TbTask>(taskVm);

            // Update user specific properties
            using(var readerRepository = _unitOfWork.ReaderRepository)
            {
                task.CreatedById = readerRepository.GetUserByUserName(taskVm.CreatedBy).FirstOrDefault().UserId;
                task.AssignedToId = readerRepository.GetUserByUserName(taskVm.AssignedTo).FirstOrDefault().UserId;
            }

            var taskRepository = _unitOfWork.TaskRepository;
            taskRepository.Add(task);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Updates task data in the database
        /// </summary>
        /// <param name="id">task ID</param>
        /// <param name="taskVm">Input task data</param>
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

            // Execute mapping from the view model to the domain object
            task = _mapper.Map<TaskVm, TbTask>(taskVm, task);

            // Update user specific property
            using(var readerRepository = _unitOfWork.ReaderRepository)
            {
                task.AssignedToId = readerRepository.GetUserByUserName(taskVm.AssignedTo).FirstOrDefault().UserId;
            }

            taskRepository.Update(task);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Removes a task from the database
        /// </summary>
        /// <param name="id">task ID</param>
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
