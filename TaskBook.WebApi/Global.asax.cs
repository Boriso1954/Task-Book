using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace TaskBook.WebApi
{
    public class WebApiApplication: System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();

            // Enables attribute routing
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
