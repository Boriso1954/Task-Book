namespace TaskBook.TestDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TaskBook.Services.AuthManagers;
    using TaskBook.DataAccessLayer.Properties;
    using TaskBook.DomainModel;

    internal sealed class ConfigurationTest: DbMigrationsConfiguration<TaskBookDbContextTest>
    {
        public ConfigurationTest()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TaskBookDbContextTest context)
        {
            CreateSP(context);
            PopulateDb(context);
        }

        private void CreateSP(TaskBookDbContextTest context)
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

            using (var userManager = new TbUserManager(new UserStore<TbUser>(context)))
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
