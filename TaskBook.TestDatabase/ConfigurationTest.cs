namespace TaskBook.TestDatabase
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TaskBook.DataAccessLayer.AuthManagers;
    using TaskBook.DomainModel;

    internal sealed class ConfigurationTest: DbMigrationsConfiguration<TaskBookDbContextTest>
    {
        public ConfigurationTest()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TaskBookDbContextTest context)
        {
            string sql = string.Empty;

            try
            {
                // Drop stored procedures
                sql = TaskBook.DataAccessLayer.Properties.Resources.DropSP;
                context.Database.ExecuteSqlCommand(sql);
            }
            catch (SqlException)
            {
                // Do nothing if SP does not exist
            }

            // Create stored procedures
            sql = TaskBook.DataAccessLayer.Properties.Resources.CreateSP;
            context.Database.ExecuteSqlCommand(sql);

            PopulateDb(context);
        }

        private void PopulateDb(TaskBookDbContextTest context)
        {
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

            var roles = new List<TbRole>()
            {
                new TbRole()
                {
                     Name = "Admin",
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
                     Name = "Manager",
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
                     Name = "AdvancedUser",
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
                     Name = "User",
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

            const string userName = "Admin";
            const string password = "admin1";
            const string email = "admin@example.com";

            using (var userManager = new TbUserManager(new UserStore<TbUser>(context)))
            {
                var user = userManager.FindByName(userName);
                if(user == null)
                {
                    user = new TbUser()
                    {
                        UserName = userName,
                        Email = email,
                        FirstName = "Admin",
                        LastName = "Admin",
                    };
                    userManager.Create(user, password);
                }

                var adminRole = roles.First(r => r.Name == "Admin");
                var rolesForUser = userManager.GetRoles(user.Id);
                if(!rolesForUser.Contains(adminRole.Name))
                {
                    var result = userManager.AddToRole(user.Id, adminRole.Name);
                }
            }
        }
    }
}
