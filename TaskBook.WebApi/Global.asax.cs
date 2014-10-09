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

            // Enable Web API 2 configuration
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // NOTE: This commented code will NOT work for Web API 2.0.
            //WebApiConfig.Register(GlobalConfiguration.Configuration);

            // Other configurations: routes, bundles, minifications
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // This line is used only in the debug mode to test bundle and minification
            // Commented out it in the production mode. The minification enables by the debug attribute in the web.config
            //BundleTable.EnableOptimizations = true;
        }
    }
}
