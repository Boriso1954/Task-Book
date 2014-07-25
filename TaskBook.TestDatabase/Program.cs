using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DataAccessLayer.Migrations;
using TaskBook.DomainModel;

namespace TaskBook.TestDatabase
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started.");

            InitializeDataStore();

            Console.WriteLine("Completed. Press any key to exit.");
            Console.ReadKey();
        }

        private static void InitializeDataStore()
        {
            // Delete database if it exists
            using(var db = new TaskBookDbContextTest())
            {
                if(db.Database.Exists())
                {
                    if(db.Database.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Close();
                        Thread.Sleep(500);
                    }
                    db.Database.Delete();
                }
            }

            // Migrtae to the latest schema and add initial data 
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TaskBookDbContextTest, ConfigurationTest>());
            var configuration = new ConfigurationTest();
            var migrator = new DbMigrator(configuration);
            migrator.Configuration.AutomaticMigrationDataLossAllowed = true;
            migrator.Update();
            
            // Add test data

            // Projects
            using(var db = new TaskBookDbContextTest())
            {
                var projects = new List<Project>()
                {
                    new Project()
                    {
                        Title = "Project 1",
                        CreatedDate = DateTimeOffset.UtcNow
                    },
                    new Project()
                    {
                        Title = "Project 2",
                        CreatedDate = DateTimeOffset.UtcNow
                    }
                };
                projects.ForEach(x => db.Projects.AddOrUpdate(y => y.Title, x));
                db.SaveChanges();

                long project1Id = projects.First(x => x.Title == "Project 1").Id;
                long project2Id = projects.First(x => x.Title == "Project 2").Id;

                // Users
                var users = new List<TbUser>()
                {
                    new TbUser()
                    {
                        UserName = "Manager1",
                        Email = "manager1@taskbook.com",
                        FirstName = "Manager 1",
                        LastName = "Manager 1",
                        ProjectId = project1Id
                    },
                    new TbUser()
                    {
                        UserName = "AdvancedUser11",
                        Email = "AdvancedUser11@taskbook.com",
                        FirstName = "AdvancedUser11",
                        LastName = "AdvancedUser11",
                        ProjectId = project1Id
                    },
                    new TbUser()
                    {
                        UserName = "AdvancedUser21",
                        Email = "AdvancedUser21@taskbook.com",
                        FirstName = "AdvancedUser21",
                        LastName = "AdvancedUser21",
                        ProjectId = project1Id
                    },
                    new TbUser()
                    {
                        UserName = "User11",
                        Email = "User11@taskbook.com",
                        FirstName = "User11",
                        LastName = "User11",
                        ProjectId = project1Id
                    },
                    new TbUser()
                    {
                        UserName = "User21",
                        Email = "User21@taskbook.com",
                        FirstName = "User21",
                        LastName = "User21",
                        ProjectId = project1Id
                    },
                    new TbUser()
                    {
                        UserName = "Manager2",
                        Email = "manager2@taskbook.com",
                        FirstName = "Manager 2",
                        LastName = "Manager 2",
                        ProjectId = project2Id
                    },
                    new TbUser()
                    {
                        UserName = "AdvancedUser12",
                        Email = "AdvancedUser12@taskbook.com",
                        FirstName = "AdvancedUser12",
                        LastName = "AdvancedUser12",
                        ProjectId = project2Id
                    },
                     new TbUser()
                    {
                        UserName = "User12",
                        Email = "User12@taskbook.com",
                        FirstName = "User12",
                        LastName = "User12",
                        ProjectId = project2Id
                    },
                    new TbUser()
                    {
                        UserName = "User22",
                        Email = "User22@taskbook.com",
                        FirstName = "User22",
                        LastName = "User22",
                        ProjectId = project2Id
                    }
                };
                users.ForEach(x => db.Users.AddOrUpdate(y => y.UserName, x));
                db.SaveChanges();

                string manager1Id = users.First(x => x.UserName == "Manager1").Id;
                string advancedUser11Id = users.First(x => x.UserName == "AdvancedUser11").Id;
                string advancedUser21Id = users.First(x => x.UserName == "AdvancedUser21").Id;
                string user11Id = users.First(x => x.UserName == "User11").Id;
                string user21Id = users.First(x => x.UserName == "User21").Id;

                string manager2Id = users.First(x => x.UserName == "Manager2").Id;
                string advancedUser12Id = users.First(x => x.UserName == "AdvancedUser12").Id;
                string user12Id = users.First(x => x.UserName == "User12").Id;
                string user22Id = users.First(x => x.UserName == "User22").Id;

                // Add users to role
                using(var userManager = new TbUserManager(new UserStore<TbUser>(db)))
                {
                    foreach(var user in users)
                    {
                        string roleName = GetRoleFromUserName(user);
                        var role = db.Roles.First(r => r.Name == roleName);
                        var rolesForUser = userManager.GetRoles(user.Id);
                        if(!rolesForUser.Contains(role.Name))
                        {
                            var result = userManager.AddToRole(user.Id, role.Name);
                        }
                    }
                }

                // Add Tasks
                var tasks = new List<TbTask>()
                {
                    // Project 1
                    new TbTask()
                    {
                         Title  ="Task11",
                         Description = "Task11 description",
                         CreatedDate = DateTimeOffset.UtcNow,
                         CreatedById = manager1Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(5),
                         Status = TbTaskStatus.New,
                         AssigneToId = advancedUser11Id
                    },
                    new TbTask()
                    {
                         Title  ="Task21",
                         Description = "Task21 description",
                         CreatedDate = DateTimeOffset.UtcNow.AddDays(-1),
                         CreatedById = manager1Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(3),
                         Status = TbTaskStatus.InProgress,
                         AssigneToId = advancedUser21Id
                    },
                    new TbTask()
                    {
                         Title  ="Task31",
                         Description = "Task31 description",
                         CreatedDate = DateTimeOffset.UtcNow,
                         CreatedById = manager1Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(7),
                         Status = TbTaskStatus.InProgress,
                         AssigneToId = user11Id
                    },
                    new TbTask()
                    {
                         Title  ="Task41",
                         Description = "Task41 description",
                         CreatedDate = DateTimeOffset.UtcNow.AddDays(-5),
                         CreatedById = manager1Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(-1),
                         Status = TbTaskStatus.Completed,
                         AssigneToId = user11Id
                    },
                    new TbTask()
                    {
                         Title  ="Task51",
                         Description = "Task51 description",
                         CreatedDate = DateTimeOffset.UtcNow,
                         CreatedById = manager1Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(4),
                         Status = TbTaskStatus.New,
                         AssigneToId = user21Id
                    },

                    // Project 2
                    new TbTask()
                    {
                         Title  ="Task12",
                         Description = "Task12 description",
                         CreatedDate = DateTimeOffset.UtcNow,
                         CreatedById = manager2Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(7),
                         Status = TbTaskStatus.InProgress,
                         AssigneToId = manager2Id
                    },
                    new TbTask()
                    {
                         Title  ="Task22",
                         Description = "Task22 description",
                         CreatedDate = DateTimeOffset.UtcNow,
                         CreatedById = manager2Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(4),
                         Status = TbTaskStatus.New,
                         AssigneToId = advancedUser12Id
                    },
                    new TbTask()
                    {
                         Title  ="Task32",
                         Description = "Task32 description",
                         CreatedDate = DateTimeOffset.UtcNow.AddDays(-5),
                         CreatedById = manager2Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(-1),
                         Status = TbTaskStatus.Completed,
                         AssigneToId = advancedUser12Id
                    },
                    new TbTask()
                    {
                         Title  ="Task42",
                         Description = "Task42 description",
                         CreatedDate = DateTimeOffset.UtcNow.AddDays(-3),
                         CreatedById = manager2Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(-1),
                         Status = TbTaskStatus.Completed,
                         AssigneToId = user12Id
                    },
                    new TbTask()
                    {
                         Title  ="Task52",
                         Description = "Task52 description",
                         CreatedDate = DateTimeOffset.UtcNow.AddDays(-1),
                         CreatedById = manager2Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(-3),
                         Status = TbTaskStatus.InProgress,
                         AssigneToId = user12Id
                    },
                    new TbTask()
                    {
                         Title  ="Task62",
                         Description = "Task62 description",
                         CreatedDate = DateTimeOffset.UtcNow,
                         CreatedById = manager2Id,
                         DueDate = DateTimeOffset.UtcNow.AddDays(8),
                         Status = TbTaskStatus.New,
                         AssigneToId = user22Id
                    }
                };
                tasks.ForEach(x => db.Tasks.AddOrUpdate(y => y.Title, x));
                db.SaveChanges();
            }
        }

        private static string GetRoleFromUserName(TbUser user)
        {
            string userName = user.UserName.ToUpper();
            if(userName.StartsWith("ADMIN"))
            {
                return RoleKey.Admin;
            }
            else if(userName.StartsWith("MANAGER"))
            {
                return RoleKey.Manager;
            }
            else if(userName.StartsWith("ADVANCED"))
            {
                return RoleKey.AdvancedUser;
            }
            else if(userName.StartsWith("USER"))
            {
                return RoleKey.User;
            }
            else
            {
                throw new Exception("User's role is incorrect."); 
            }
        }
    }
}
