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
    }
}
