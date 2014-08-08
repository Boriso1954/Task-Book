using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DataAccessLayer.Exceptions;

namespace TaskBook.DataAccessLayer
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly TaskBookDbContext _db;

        public UnitOfWork(TaskBookDbContext database)
        {
            this._db = database;
        }

        public TaskBookDbContext Database
        {
            get
            {
                return this._db;
            }
        }

        public void Commit()
        {
            try
            {
                _db.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new DataAccessLayerException("Concurrency", ex);
            }
            catch(DbEntityValidationException ex)
            {
                throw new DataAccessLayerException("Validation", ex);
            }
            catch(DbUpdateException ex)
            {
                throw new DataAccessLayerException("Update", ex);
            }
            catch(Exception ex)
            {
                throw new DataAccessLayerException("Exception", ex);
            }
        }

        public void Dispose()
        {
            this._db.Dispose();
        }
    }
}
