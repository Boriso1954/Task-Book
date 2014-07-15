using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer
{
    public class TaskBookDbContext: IdentityDbContext<User>
    {
        public TaskBookDbContext()
            : base("TaskBookDbContext")
        {
            // This makes sure that the entities are retrieved as objects of their respective classes
            // instead of proxies–making debugging much easier.
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Model configurations here
        }
    }
}
