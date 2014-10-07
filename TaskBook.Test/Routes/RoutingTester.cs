using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;

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

            var controllerSelector = new DefaultHttpControllerSelector(config);
            var controllerContext = new HttpControllerContext(config, routeData, request);
            var controllerDescriptor = controllerSelector.SelectController(request);
            controllerContext.ControllerDescriptor = controllerDescriptor;
            var controllerType = controllerContext.ControllerDescriptor.ControllerType;

            var actionSelector = new ApiControllerActionSelector();
            var action = actionSelector.SelectAction(controllerContext).ActionName;

            assert(controllerType, action);
        }
    }
}
