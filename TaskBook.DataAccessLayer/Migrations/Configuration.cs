namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TaskBook.DataAccessLayer.AuthManagers;
    using TaskBook.DataAccessLayer.Properties;
    using TaskBook.DomainModel;

    /// <summary>
    /// Database migration configuration class
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<TaskBook.DataAccessLayer.TaskBookDbContext>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Populates a database
        /// </summary>
        /// <param name="context">Database context</param>
        protected override void Seed(TaskBookDbContext context)
        {
            CreateSP(context);
            PopulateDb(context);
        }

        /// <summary>
        /// Adds stored procedures to the database
        /// </summary>
        /// <param name="context">Database context</param>
        private void CreateSP(TaskBookDbContext context)
        {
            string sql = string.Empty;
            string[] commandTexts = null;

            commandTexts = Resources.DropSP.Split(new string[] { "\nGO" }, System.StringSplitOptions.RemoveEmptyEntries);
            try
            {
                // Drop stored procedures
                for(int i = 0; i < commandTexts.Length; i++)
                {
                    context.Database.ExecuteSqlCommand(commandTexts[i]);
                }
            }
            catch(SqlException ex)
            {
                // Do nothing if SP already exists
                string error = ex.Message;
            }

            // Create stored procedures
            commandTexts = Resources.CreateSP.Split(new string[] { "\nGO" }, System.StringSplitOptions.RemoveEmptyEntries);

            try
            {
                for(int i = 0; i < commandTexts.Length; i++)
                {
                    context.Database.ExecuteSqlCommand(commandTexts[i]);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error during creating SP: {0}", ex.Message);
                context.Database.Connection.Close();
                return;
            }
        }

        /// <summary>
        /// Initializes the database
        /// </summary>
        /// <param name="context"></param>
        private void PopulateDb(TaskBookDbContext context)
        {
            // Add permissions
            var permissions = new List<Permission>()
                {
                    new Permission()
                    {
                         Name = PermissionKey.ViewUsers,
                         Description = "View project users"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ManageUsers,
                         Description = "Add, modify and delete users"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ViewRolePermissions,
                         Description = "View roles and permissions"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ViewTasks,
                         Description = "View users' tasks"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ManageTasks,
                         Description = "Add, modify, and delete tasks"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ViewOwnTasks,
                         Description = "View own tasks"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ModifyOwnTasks,
                         Description = "Modify own tasks"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ModifyOwnAccount,
                         Description = "Modify own account"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ViewProjectsManagers,
                         Description = "View projects and managers"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.ManageProjects,
                         Description = "Add, modify, and delete projects"
                    },
                    new Permission()
                    {
                         Name = PermissionKey.AddManagerToProject,
                         Description = "Add manager to a project"
                    }
                };
            permissions.ForEach(x => context.Permissions.AddOrUpdate(y => y.Name, x));
            context.SaveChanges();

            // Add roles
            var roles = new List<TbRole>()
            {
                new TbRole()
                {
                     Name = RoleKey.Admin,
                     Description = "Administrator of the TaskBook application",
                     Permissions = new List<Permission>()
                     {
                         permissions.First(x => x.Name == PermissionKey.ViewProjectsManagers),
                         permissions.First(x => x.Name == PermissionKey.ManageProjects),
                         permissions.First(x => x.Name == PermissionKey.AddManagerToProject)
                     }
                },
                new TbRole()
                {
                     Name = RoleKey.Manager,
                     Description ="Project manager",
                     Permissions = new List<Permission>()
                     {
                         permissions.First(x => x.Name == PermissionKey.ViewUsers),
                         permissions.First(x => x.Name == PermissionKey.ManageUsers),
                         permissions.First(x => x.Name == PermissionKey.ViewRolePermissions),
                         permissions.First(x => x.Name == PermissionKey.ViewTasks),
                         permissions.First(x => x.Name == PermissionKey.ManageTasks),
                         permissions.First(x => x.Name == PermissionKey.ViewOwnTasks),
                         permissions.First(x => x.Name == PermissionKey.ModifyOwnTasks),
                         permissions.First(x => x.Name == PermissionKey.ModifyOwnAccount)
                     }
                },
                new TbRole()
                {
                     Name = RoleKey.AdvancedUser,
                     Description = "Advanced user in the project",
                     Permissions = new List<Permission>()
                     {
                         permissions.First(x => x.Name == PermissionKey.ViewUsers),
                         permissions.First(x => x.Name == PermissionKey.ViewRolePermissions),
                         permissions.First(x => x.Name == PermissionKey.ViewTasks),
                         permissions.First(x => x.Name == PermissionKey.ManageTasks),
                         permissions.First(x => x.Name == PermissionKey.ViewOwnTasks),
                         permissions.First(x => x.Name == PermissionKey.ModifyOwnTasks),
                         permissions.First(x => x.Name == PermissionKey.ModifyOwnAccount)
                     }
                },
                new TbRole()
                {
                     Name = RoleKey.User,
                     Description = "Mere user in the project",
                     Permissions = new List<Permission>()
                     {
                         permissions.First(x => x.Name == PermissionKey.ViewOwnTasks),
                         permissions.First(x => x.Name == PermissionKey.ModifyOwnTasks),
                         permissions.First(x => x.Name == PermissionKey.ModifyOwnAccount)
                     }
                }
            };

            using(var roleManager = new TbRoleManager(new RoleStore<TbRole>(context)))
            {
                foreach(var role in roles)
                {
                    bool roleExists = roleManager.RoleExists(role.Name);
                    if(!roleExists)
                    {
                        roleManager.Create(role);
                    }
                }
            }

            // Add TaskBook administrator
            using(var userManager = new TbUserManager(new UserStore<TbUser>(context)))
            {
                string adminName = "Admin1";
                var user = userManager.FindByName(adminName);
                if(user == null)
                {
                    user = new TbUser()
                    {
                        UserName = adminName,
                        Email = "admin1@taskbook.com",
                        FirstName = "Admin1",
                        LastName = "Admin1",
                    };
                    userManager.Create(user, "admin1");
                }

                var role = roles.First(r => r.Name == RoleKey.Admin);
                var rolesForUser = userManager.GetRoles(user.Id);
                if(!rolesForUser.Contains(role.Name))
                {
                    var result = userManager.AddToRole(user.Id, role.Name);
                }

                // That is a system account
                string notAssignedName = "NotAssigned"; // W/o spaces!
                user = userManager.FindByName(notAssignedName);
                if(user == null)
                {
                    user = new TbUser()
                    {
                        UserName = notAssignedName,
                        FirstName = "Not Assigned",
                        LastName = "Not Assigned",
                    };
                    userManager.Create(user, "notassigned");
                }

                role = roles.First(r => r.Name == RoleKey.Manager);
                rolesForUser = userManager.GetRoles(user.Id);
                if(!rolesForUser.Contains(role.Name))
                {
                    var result = userManager.AddToRole(user.Id, role.Name);
                }
            }
        }
    }
}
