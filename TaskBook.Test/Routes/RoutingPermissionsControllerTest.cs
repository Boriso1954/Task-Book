using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NUnit.Framework;
using TaskBook.WebApi;
using TaskBook.WebApi.Controllers;

namespace TaskBook.Test
{
    [TestFixture]
    public class RoutingPermissionsControllerTest
    {
        HttpConfiguration _config;

        [SetUp]
        public void Init()
        {
            _config = new HttpConfiguration();
            WebApiConfig.Register(_config);
            _config.EnsureInitialized();
        }

        [TearDown]
        public void Cleanup()
        {
            _config.Dispose();
        }

        [Test]
        public void Routing_PermissionsController_GetByRole()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Permissions/GetByRole/roleName");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(PermissionsController), t);
                    Assert.AreEqual("GetByRole", m);
                });
        }
    }
}
