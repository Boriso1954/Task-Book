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
            //: base("TaskBookDbContextTest") // Only for test goal

        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<TbTask> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ProjectUsers> ProjectUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Other schema configurations here
            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new ProjectUsersConfiguration());
        }

        public static TaskBookDbContext Create()
        {
            return new TaskBookDbContext();
        }
    }
}
