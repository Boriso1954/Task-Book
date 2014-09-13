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
using System.Web.Http.Routing;

namespace TaskBook.Test
{
    internal static class RoutingTester
    {
        internal static void TestRoute(HttpRequestMessage request,
            HttpConfiguration config,
            Action<Type, string> assert)
        {
            var routeData = config.Routes.GetRouteData(request);
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            var controllerContext = new HttpControllerContext(config, routeData, request);
            var controllerSelector = new DefaultHttpControllerSelector(config);

            var controllerDescriptor = controllerSelector.SelectController(request);

            controllerContext.ControllerDescriptor = controllerDescriptor;

            var actionSelector = new ApiControllerActionSelector();
            var action = actionSelector.SelectAction(controllerContext).ActionName;
            
            var controllerType = controllerContext.ControllerDescriptor.ControllerType;

            assert(controllerType, action);
        }
    }
}
