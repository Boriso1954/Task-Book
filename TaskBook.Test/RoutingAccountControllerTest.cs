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
    public sealed class RoutingAccountControllerTest
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
        public void Routing_AccountController_GetUserByUserName()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Account/GetUserByUserName/user1");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(AccountController), t);
                    Assert.AreEqual("GetUserByUserName", m);
                });
        }

        [Test]
        public void Routing_AccountController_GetUsersWithRolesByProjectId()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Account/GetUsersWithRolesByProjectId/1");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(AccountController), t);
                    Assert.AreEqual("GetUsersWithRolesByProjectId", m);
                });
        }

        [Test]
        public void Routing_AccountController_GetUsersByProjectId()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Account/GetUsersByProjectId/1");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(AccountController), t);
                    Assert.AreEqual("GetUsersByProjectId", m);
                });
        }

        [Test]
        public void Routing_AccountController_ForgotPassword()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Account/ForgotPassword");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(AccountController), t);
                    Assert.AreEqual("ForgotPassword", m);
                });
        }

        [Test]
        public void Routing_AccountController_ResetPassword()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Account/ResetPassword");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(AccountController), t);
                    Assert.AreEqual("ResetPassword", m);
                });
        }

        [Test]
        public void Routing_AccountController_AddUserAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Account/AddUser");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(AccountController), t);
                    Assert.AreEqual("AddUserAsync", m);
                });
        }

        [Test]
        public void Routing_AccountController_UpdateUser()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/Account/UpdateUser/userId");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(AccountController), t);
                    Assert.AreEqual("UpdateUser", m);
                });
        }

        [Test]
        public void Routing_AccountController_DeleteUser()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost/api/Account/DeleteUser/userId");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(AccountController), t);
                    Assert.AreEqual("DeleteUser", m);
                });
        }
    }
}
