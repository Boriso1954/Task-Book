using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Web.Http;
using Unity.WebApi;
using TaskBook.DataAccessLayer;
using Microsoft.AspNet.Identity;
using TaskBook.DomainModel;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System;
using TaskBook.Services;
using TaskBook.Services.Interfaces;
using TaskBook.WebApi.Controllers;

namespace TaskBook.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = BuildUnityContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            //LoadDependencyAssemblies();
            var container = new UnityContainer();

            container.LoadConfiguration();

            //var accountInjectionConstructor = new InjectionConstructor(new TaskBookDbContext());
            //container.RegisterType<IUserStore<TbUser>, UserStore<TbUser>>(accountInjectionConstructor);
            //container.RegisterType<TaskBook.Services.Interfaces.ITbRoleService, TbRoleService>();

            return container;
        }

        private static void LoadDependencyAssemblies()
        {
            UnityConfigurationSection section = ConfigurationManager.GetSection(UnityConfigurationSection.SectionName) as UnityConfigurationSection;
            if(section == null)
            {
                throw new ConfigurationErrorsException("Unable to locate Unity configuration section.");
            }

            foreach(string assemblyName in section.Assemblies.Select(element => element.Name))
            {
                try
                {
                    Assembly.Load(assemblyName);

                }
                catch(Exception ex)
                {
                    throw new ConfigurationErrorsException("Unable to load required assembly specified in Unity configuration section.  Assembly: " + assemblyName, ex);
                }
            }
        }
    }
}