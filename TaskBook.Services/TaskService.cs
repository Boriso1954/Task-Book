using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessReader;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    public sealed class TaskService: ITaskService
    {
        private readonly ReaderRepository _readerRepository;

        public TaskService(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
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

        public void Dispose()
        {
            _readerRepository.Dispose();
        }
    }
}
