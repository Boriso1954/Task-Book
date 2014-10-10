using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskBook.DataAccessLayer.Configurations;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer
{
    /// <summary>
    /// Database context class which also provides operations to manipulate users and roles
    /// </summary>
    public sealed class TaskBookDbContext: IdentityDbContext<TbUser>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaskBookDbContext()
            : base("TaskBookDbContext")
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

        /// <summary>
        /// Creates the Database context instance
        /// </summary>
        /// <returns>Database context</returns>
        public static TaskBookDbContext Create()
        {
            return new TaskBookDbContext();
        }
    }
}
