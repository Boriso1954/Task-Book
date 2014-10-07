using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using TaskBook.DomainModel.Mapping;

namespace TaskBook.WebApi
{
    public class WebApiApplication: System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Unity registrator
            UnityConfig.RegisterComponents();

            // Initialize automapper
            AutoMapperConfiguration.Initialize();

            // Enables attribute routing
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // The line below duplicates the above line
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //BundleTable.EnableOptimizations = true;
        }
    }
}
