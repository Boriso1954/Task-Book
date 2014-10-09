﻿using System.Web.Http;
using System.Web.Http.Tracing;

namespace TaskBook.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // For Web API 2 - enable attribute routing
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name : "DefaultApi",
                routeTemplate : "api/{controller}/{id}",
                defaults : new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute
            (
                name : "ActionApi",
                routeTemplate : "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var tracer = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ITraceWriter)) as ITraceWriter;
            if(tracer != null)
            {
                config.Services.Replace(typeof(ITraceWriter), tracer);
            }
        }
    }
}
