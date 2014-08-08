using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessLayer
{
    public interface IUnitOfWork: IDisposable
    {
        TaskBookDbContext Database { get; }
        void Commit();
    }
}
