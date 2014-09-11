using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;

namespace TaskBook.Test
{
    internal static class RoutingTester
    {
        internal static void TestRoute(HttpConfiguration config,
            HttpRequestMessage request,
            Action<Type, string> callback)
        {
            var controllerContextData = GetControllerContextData(config, request);

            var actionSelector = new ApiControllerActionSelector();
            var action = actionSelector.SelectAction(controllerContextData.HttpControllerContext).ActionName;
            var controllerType = controllerContextData.HttpControllerContext.ControllerDescriptor.ControllerType;

            callback(controllerType, action);

        }

        internal static ControllerConextData GetControllerContextData(HttpConfiguration config,
            HttpRequestMessage request)
        {
            var routeData = config.Routes.GetRouteData(request);
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            var controllerContext = new HttpControllerContext(config, routeData, request);
            var controllerSelector = new DefaultHttpControllerSelector(config);
            var controllerDescriptor = controllerSelector.SelectController(request);

            return new ControllerConextData()
            {
                HttpControllerContext = controllerContext,
                HttpControllerDescriptor = controllerDescriptor
            };
        }
    }

    internal sealed class ControllerConextData
    {
        internal HttpControllerContext HttpControllerContext { get; set; }
        internal HttpControllerDescriptor HttpControllerDescriptor { get; set; }
    }
}
