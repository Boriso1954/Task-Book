using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskBook.DataAccessLayer.Configurations;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer
{
    public class TaskBookDbContext: IdentityDbContext<TbUser>
    {
        public TaskBookDbContext()
            : base("TaskBookDbContext")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        //public DbSet<TbTask> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Other schema configurations here
            //modelBuilder.Configurations.Add(new UserConfiguration());
        }

        public static TaskBookDbContext Create()
        {
            return new TaskBookDbContext();
        }
    }
}
