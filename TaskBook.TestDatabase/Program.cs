using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
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
            bool create = false;
            bool demo = false;
            bool help = false;

            foreach(var s in args)
            {
                create = s.ToUpper().Contains("CREATE");
                demo = s.ToUpper().Contains("DEMO");
            }
            help = !(create || demo);
            if(help)
            {
                Console.WriteLine("HELP: TaskBookDatabase.exe [Create] [Demo]");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Started.");
            
            InitializeDataStore(demo);

            Console.WriteLine("Completed. Press any key to exit.");
            Console.ReadKey();
        }

        private static void InitializeDataStore(bool demo)
        {
            // Delete database if it exists
            using(var db = new TaskBookDbContext())
            {
                if(db.Database.Exists())
                {
                    Console.WriteLine("Attempting to delete the existing database...");
                    if(db.Database.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        db.Database.Connection.Close();
                        Thread.Sleep(500);
                    }
                    try
                    {
                        db.Database.Delete();
                    }
                    catch(SqlException ex)
                    {
                        Console.WriteLine("Unable to delete the existing database.\nERROR: {0}\n Try to delete the database manually.", ex.Message);
                        return;
                    }
                }
            }

            Console.WriteLine("Migrating to the latest schema and adding the initial data...");

            try
            {
                // Migrate to the latest schema and add initial data 
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<TaskBookDbContext, Configuration>());
                var configuration = new Configuration();
                var migrator = new DbMigrator(configuration);
                migrator.Configuration.AutomaticMigrationDataLossAllowed = true;
                migrator.Update();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to perform the database migration.\nERROR: {0}", ex.Message);
                return;
            }
            Console.WriteLine("The database has been migrated");

            if(demo)
            {
                AddDemoData();
            }
        }

        private static void AddDemoData()
        {
            Console.WriteLine("Adding the demo data...");
  
            using(var db = new TaskBookDbContext())
            {
                try
                {
                    Console.WriteLine("Adding projects...");

                    // Projects
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

                    Console.WriteLine("Adding users...");

                    // Users
                    var users = new List<TbUser>()
                    {
                        new TbUser()
                        {
                            UserName = "Manager1",
                            Email = "manager1@taskbook.com",
                            FirstName = "Manager 1",
                            LastName = "Manager 1",
                        },
                        new TbUser()
                        {
                            UserName = "AdvancedUser11",
                            Email = "AdvancedUser11@taskbook.com",
                            FirstName = "AdvancedUser11",
                            LastName = "AdvancedUser11",
                        },
                        new TbUser()
                        {
                            UserName = "AdvancedUser21",
                            Email = "AdvancedUser21@taskbook.com",
                            FirstName = "AdvancedUser21",
                            LastName = "AdvancedUser21",
                        },
                        new TbUser()
                        {
                            UserName = "User11",
                            Email = "User11@taskbook.com",
                            FirstName = "User11",
                            LastName = "User11",
                        },
                        new TbUser()
                        {
                            UserName = "User21",
                            Email = "User21@taskbook.com",
                            FirstName = "User21",
                            LastName = "User21",
                        },
                        new TbUser()
                        {
                            UserName = "Manager2",
                            Email = "manager2@taskbook.com",
                            FirstName = "Manager 2",
                            LastName = "Manager 2",
                        },
                        new TbUser()
                        {
                            UserName = "AdvancedUser12",
                            Email = "AdvancedUser12@taskbook.com",
                            FirstName = "AdvancedUser12",
                            LastName = "AdvancedUser12",
                        },
                         new TbUser()
                        {
                            UserName = "User12",
                            Email = "User12@taskbook.com",
                            FirstName = "User12",
                            LastName = "User12",
                        },
                        new TbUser()
                        {
                            UserName = "User22",
                            Email = "User22@taskbook.com",
                            FirstName = "User22",
                            LastName = "User22",
                        }
                    };

                    // Add users, add users to role
                    using(var userManager = new TbUserManager(new UserStore<TbUser>(db)))
                    {
                        foreach(var user in users)
                        {
                            if(userManager.FindByName(user.UserName) == null)
                            {
                                userManager.Create(user, "user12");
                                string roleName = GetRoleFromUserName(user);
                                var role = db.Roles.First(r => r.Name == roleName);
                                var rolesForUser = userManager.GetRoles(user.Id);
                                if(!rolesForUser.Contains(role.Name))
                                {
                                    var result = userManager.AddToRole(user.Id, role.Name);
                                }
                            }
                        }
                    }

                    // For Project 1
                    string manager1Id = users.First(x => x.UserName == "Manager1").Id;
                    string advancedUser11Id = users.First(x => x.UserName == "AdvancedUser11").Id;
                    string advancedUser21Id = users.First(x => x.UserName == "AdvancedUser21").Id;
                    string user11Id = users.First(x => x.UserName == "User11").Id;
                    string user21Id = users.First(x => x.UserName == "User21").Id;

                    // For Project 2
                    string manager2Id = users.First(x => x.UserName == "Manager2").Id;
                    string advancedUser12Id = users.First(x => x.UserName == "AdvancedUser12").Id;
                    string user12Id = users.First(x => x.UserName == "User12").Id;
                    string user22Id = users.First(x => x.UserName == "User22").Id;

                    var projectUsers = new List<ProjectUsers>()
                    {
                        // Project 1 members
                        new ProjectUsers()
                        {
                             ProjectId = project1Id,
                             UserId = manager1Id
                        },
                        new ProjectUsers()
                        {
                             ProjectId = project1Id,
                             UserId = advancedUser11Id
                        },
                        new ProjectUsers()
                        {
                             ProjectId = project1Id,
                             UserId = advancedUser21Id
                        },
                        new ProjectUsers()
                        {
                             ProjectId = project1Id,
                             UserId = user11Id
                        },
                        new ProjectUsers()
                        {
                             ProjectId = project1Id,
                             UserId = user21Id
                        },
                        // Project 2 members
                        new ProjectUsers()
                        {
                             ProjectId = project2Id,
                             UserId = manager2Id
                        },
                        new ProjectUsers()
                        {
                             ProjectId = project2Id,
                             UserId = advancedUser12Id
                        },
                        new ProjectUsers()
                        {
                             ProjectId = project2Id,
                             UserId = user12Id
                        },
                        new ProjectUsers()
                        {
                             ProjectId = project2Id,
                             UserId = user22Id
                        },
                    };

                    projectUsers.ForEach(x => db.ProjectUsers.AddOrUpdate(y => y.Id, x));
                    db.SaveChanges();

                    Console.WriteLine("Adding tasks...");

                    // Add Tasks
                    var tasks = new List<TbTask>()
                    {
                        // Project 1
                        new TbTask()
                        {
                             Title  ="Task11",
                             ProjectId = project1Id,
                             Description = "Task11 description",
                             CreatedDate = DateTimeOffset.UtcNow,
                             CreatedById = manager1Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(5),
                             Status = TbTaskStatus.New,
                             AssignedToId = advancedUser11Id
                        },
                        new TbTask()
                        {
                             Title  ="Task21",
                             ProjectId = project1Id,
                             Description = "Task21 description",
                             CreatedDate = DateTimeOffset.UtcNow.AddDays(-1),
                             CreatedById = manager1Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(3),
                             Status = TbTaskStatus.InProgress,
                             AssignedToId = advancedUser21Id
                        },
                        new TbTask()
                        {
                             Title  ="Task31",
                             ProjectId = project1Id,
                             Description = "Task31 description",
                             CreatedDate = DateTimeOffset.UtcNow,
                             CreatedById = manager1Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(7),
                             Status = TbTaskStatus.InProgress,
                             AssignedToId = user11Id
                        },
                        new TbTask()
                        {
                             Title  ="Task41",
                             ProjectId = project1Id,
                             Description = "Task41 description",
                             CreatedDate = DateTimeOffset.UtcNow.AddDays(-5),
                             CreatedById = manager1Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(-1),
                             Status = TbTaskStatus.Completed,
                             AssignedToId = user11Id,
                             CompletedDate = DateTimeOffset.UtcNow.AddDays(-1)
                        },
                        new TbTask()
                        {
                             Title  ="Task51",
                             ProjectId = project1Id,
                             Description = "Task51 description",
                             CreatedDate = DateTimeOffset.UtcNow,
                             CreatedById = manager1Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(4),
                             Status = TbTaskStatus.New,
                             AssignedToId = user21Id
                        },

                        // Project 2
                        new TbTask()
                        {
                             Title  ="Task12",
                             ProjectId = project2Id,
                             Description = "Task12 description",
                             CreatedDate = DateTimeOffset.UtcNow,
                             CreatedById = manager2Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(7),
                             Status = TbTaskStatus.InProgress,
                             AssignedToId = manager2Id
                        },
                        new TbTask()
                        {
                             Title  ="Task22",
                             ProjectId = project2Id,
                             Description = "Task22 description",
                             CreatedDate = DateTimeOffset.UtcNow,
                             CreatedById = manager2Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(4),
                             Status = TbTaskStatus.New,
                             AssignedToId = advancedUser12Id
                        },
                        new TbTask()
                        {
                             Title  ="Task32",
                             ProjectId = project2Id,
                             Description = "Task32 description",
                             CreatedDate = DateTimeOffset.UtcNow.AddDays(-5),
                             CreatedById = manager2Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(-1),
                             Status = TbTaskStatus.Completed,
                             AssignedToId = advancedUser12Id,
                             CompletedDate = DateTimeOffset.UtcNow.AddDays(-1)
                        },
                        new TbTask()
                        {
                             Title  ="Task42",
                             ProjectId = project2Id,
                             Description = "Task42 description",
                             CreatedDate = DateTimeOffset.UtcNow.AddDays(-3),
                             CreatedById = manager2Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(-1),
                             Status = TbTaskStatus.Completed,
                             AssignedToId = user12Id,
                             CompletedDate = DateTimeOffset.UtcNow.AddDays(-1)
                        },
                        new TbTask()
                        {
                             Title  ="Task52",
                             ProjectId = project2Id,
                             Description = "Task52 description",
                             CreatedDate = DateTimeOffset.UtcNow.AddDays(-1),
                             CreatedById = manager2Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(-3),
                             Status = TbTaskStatus.InProgress,
                             AssignedToId = user12Id
                        },
                        new TbTask()
                        {
                             Title  ="Task62",
                             ProjectId = project2Id,
                             Description = "Task62 description",
                             CreatedDate = DateTimeOffset.UtcNow,
                             CreatedById = manager2Id,
                             DueDate = DateTimeOffset.UtcNow.AddDays(8),
                             Status = TbTaskStatus.New,
                             AssignedToId = user22Id
                        }
                    };
                    tasks.ForEach(x => db.Tasks.AddOrUpdate(y => y.Title, x));
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error during the demo data adding.\nERROR: {0}", ex.Message);
                    return;
                }

                Console.WriteLine("Demo data has been added");
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
