namespace TaskBook.DataAccessLayer.AuthMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using TaskBook.DataAccessLayer.AuthManagers;
    using TaskBook.DataAccessLayer.Repositories;
    using TaskBook.DomainModel;

    internal sealed class Configuration : DbMigrationsConfiguration<AuthDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"AuthMigrations";
        }

        protected override void Seed(AuthDbContext context)
        {
            var userManager = new TbUserManager(new UserStore<TbUser>(context));
            var roleManager = new TbRoleManager(new RoleStore<TbRole>(context));

            List<TbRole> roles = new List<TbRole>()
            {
                new TbRole()
                {
                     Name = "Admin",
                     Description="Administrator of the TaskBook application"
                },
                new TbRole()
                {
                     Name = "Manager",
                     Description="Project manager"
                },
                new TbRole()
                {
                     Name = "AdvancedUser",
                     Description="Advanced user in the project"
                },
                new TbRole()
                {
                     Name = "User",
                     Description="Mere user in the project"
                }
            };

            foreach(var role in roles)
            {
                bool roleExists = roleManager.RoleExists(role.Name);
                if(!roleExists)
                {
                    roleManager.Create(role);
                }
            }

            const string userName = "Admin";
            const string password = "admin1";
            const string email = "admin@example.com";

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

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
