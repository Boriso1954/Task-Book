using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.WebApi;
using System.Configuration;
using System;

namespace TaskBook.Test
{
    internal static class UnityTestConfig
    {
        internal static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.LoadConfiguration();
            return container;
        }
    }
}
