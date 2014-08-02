using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Web.Http;
using Unity.WebApi;
using TaskBook.DataAccessLayer;
using Microsoft.AspNet.Identity;
using TaskBook.DomainModel;
using Microsoft.AspNet.Identity.EntityFramework;

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
            var container = new UnityContainer();

            container.LoadConfiguration();

            var accountInjectionConstructor = new InjectionConstructor(new TaskBookDbContext());
            container.RegisterType<IUserStore<TbUser>, UserStore<TbUser>>(accountInjectionConstructor);
            //container.RegisterType<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>(accountInjectionConstructor);

            return container;
        }
    }
}