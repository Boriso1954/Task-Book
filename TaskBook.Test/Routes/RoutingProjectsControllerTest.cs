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
    public class RoutingProjectsControllerTest
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
        public void Routing_ProjectsController_GetProjectsAndManagers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Projects/GetProjectsAndManagers");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(ProjectsController), t);
                    Assert.AreEqual("GetProjectsAndManagers", m);
                });
        }

        [Test]
        public void Routing_ProjectsController_GetProjectsAndManagersByProjectId()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Projects/GetProjectsAndManagers/projectId");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(ProjectsController), t);
                    Assert.AreEqual("GetProjectsAndManagers", m);
                });
        }

        [Test]
        public void Routing_ProjectsController_GetProjectById()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Projects/GetProjectById/id");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(ProjectsController), t);
                    Assert.AreEqual("GetProjectById", m);
                });
        }

        [Test]
        public void Routing_ProjectsController_AddProject()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Projects/AddProject");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(ProjectsController), t);
                    Assert.AreEqual("AddProject", m);
                });
        }

        [Test]
        public void Routing_ProjectsController_UpdateProject()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/Projects/UpdateProject/1");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(ProjectsController), t);
                    Assert.AreEqual("UpdateProject", m);
                });
        }

        [Test]
        public void Routing_ProjectsController_DeleteProject()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost/api/Projects/DeleteProject/1");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(ProjectsController), t);
                    Assert.AreEqual("DeleteProject", m);
                });
        }
    }
}
