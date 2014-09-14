using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;
using TaskBook.WebApi;
using TaskBook.WebApi.Controllers;

namespace TaskBook.Test
{
    [TestFixture]
    public sealed class RoutingRolesControllerTest
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
        public void Routing_RolesController_GetRolesByUserName()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Roles/GetRolesByUserName/userName");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(RolesController), t);
                    Assert.AreEqual("GetRolesByUserName", m);
                });
        }

        [Test]
        public void Routing_RolesController_GetRolesByUserId()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Roles/GetRolesByUserId/id");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(RolesController), t);
                    Assert.AreEqual("GetRolesByUserId", m);
                });
        }
    }
}
