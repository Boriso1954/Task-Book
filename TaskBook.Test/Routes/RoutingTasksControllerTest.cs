using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;
using TaskBook.WebApi;
using TaskBook.WebApi.Controllers;

namespace TaskBook.Test
{
     [TestFixture]
    public sealed class RoutingTasksControllerTest
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
        public void Routing_TasksController_GetTasks()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Tasks/GetTasks");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(TasksController), t);
                    Assert.AreEqual("GetTasks", m);
                });
        }

        [Test]
        public void Routing_TasksController_GetTasksByProjectId()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Tasks/GetTasks/1");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(TasksController), t);
                    Assert.AreEqual("GetTasks", m);
                });
        }

        [Test]
        public void Routing_TasksController_GetTasksByUserName()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Tasks/GetTasks/user");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(TasksController), t);
                    Assert.AreEqual("GetTasks", m);
                });
        }

        [Test]
        public void Routing_TasksController_GetTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Tasks/GetTask/1");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(TasksController), t);
                    Assert.AreEqual("GetTask", m);
                });
        }

        [Test]
        public void Routing_TasksController_AddTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Tasks/AddTask");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(TasksController), t);
                    Assert.AreEqual("AddTask", m);
                });
        }

        [Test]
        public void Routing_TasksController_UpdateTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/Tasks/UpdateTask");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(TasksController), t);
                    Assert.AreEqual("UpdateTask", m);
                });
        }

        [Test]
        public void Routing_TasksController_DeleteTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost/api/Tasks/DeleteTask");
            RoutingTester.TestRoute(request, _config,
                (t, m) =>
                {
                    Assert.AreEqual(typeof(TasksController), t);
                    Assert.AreEqual("DeleteTask", m);
                });
        }
    }
}
