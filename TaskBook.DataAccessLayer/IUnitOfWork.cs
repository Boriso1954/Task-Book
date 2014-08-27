using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DataAccessLayer.Repositories.Interfaces;

namespace TaskBook.DataAccessLayer
{
    public interface IUnitOfWork: IDisposable
    {
        IProjectRepository ProjectRepository { get; }
        IProjectUsersRepository ProjectUsersRepository { get; }
        ITaskRepository TaskRepository { get; }
        IReaderRepository ReaderRepository { get; }
        TaskBookDbContext DbContext { get; }
        void Commit();
        
    }
}
