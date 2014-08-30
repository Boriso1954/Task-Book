using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel.ViewModels;

namespace TaskBook.Services.Interfaces
{
    public interface ITaskService: IDisposable
    {
        IQueryable<TaskVm> GetTasks(long? projectId = null);
        TaskVm GetTask(long id);
        IQueryable<TaskVm> GetTasksByUserName(string userName);
        void AddTask(TaskVm taskVm);
        void UpdateTask(long id, TaskVm taskVm);
        void DeleteTask(long id);
    }
}
