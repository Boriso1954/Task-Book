using System;
using System.Linq;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.Services.Interfaces
{
    /// <summary>
    /// Defines the TaskService operations
    /// </summary>
    public interface ITaskService: IDisposable
    {
        /// <summary>
        /// Returns the task data
        /// </summary>
        /// <param name="id">Ttask ID</param>
        /// <returns>Task data</returns>
        TaskVm GetTask(long id);

        /// <summary>
        /// Returns tasks for the project
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Tasks for the project</returns>
        /// <remarks>If the project ID does not specify the method returns tasks for all the projects</remarks>
        IQueryable<TaskVm> GetTasks(long? projectId = null);

        /// <summary>
        /// Returns user's tasks
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User's tasks</returns>
        IQueryable<TaskVm> GetTasksByUserName(string userName);

        /// <summary>
        /// Adds a task to the database
        /// </summary>
        /// <param name="taskVm">Task data</param>
        void AddTask(TaskVm taskVm);

        /// <summary>
        /// Updates task data in the database
        /// </summary>
        /// <param name="id">task ID</param>
        /// <param name="taskVm">Input task data</param>
        void UpdateTask(long id, TaskVm taskVm);

        /// <summary>
        /// Removes a task from the database
        /// </summary>
        /// <param name="id">Task ID</param>
        void DeleteTask(long id);
    }
}
