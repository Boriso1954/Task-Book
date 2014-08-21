using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Repositories
{
    public class TaskRepository: Repository<TbTask>, ITaskRepository
    {
        public TaskRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
