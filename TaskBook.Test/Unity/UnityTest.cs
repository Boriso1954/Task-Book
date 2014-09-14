using System.Web.Http.Tracing;
using Microsoft.Practices.Unity;
using NLog.Mvc;
using NUnit.Framework;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DataAccessLayer.Repositories;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel.Mapping;
using TaskBook.Services;
using TaskBook.Services.Interfaces;
using TaskBook.WebApi.Tracer;

namespace TaskBook.Test
{
    [TestFixture]
    public sealed class UnityTest
    {
        IUnityContainer _container; 

        [SetUp]
        public void Init()
        {
            _container = UnityTestConfig.BuildUnityContainer();
        }

        [TearDown]
        public void Cleanup()
        {
            _container.Dispose();
        }

        [Test]
        public void UnityTest_Logger_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<ILogger>();
            var instance2 = _container.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<Logger>(instance1);
            Assert.IsInstanceOf<Logger>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_TraceWriter_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<ITraceWriter>();
            var instance2 = _container.Resolve<ITraceWriter>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<NLogTracer>(instance1);
            Assert.IsInstanceOf<NLogTracer>(instance2);
            Assert.AreSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_Mapper_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IMapper>();
            var instance2 = _container.Resolve<IMapper>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<ViewModelToDomainMapper>(instance1);
            Assert.IsInstanceOf<ViewModelToDomainMapper>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_TaskBookDbContext_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<TaskBookDbContext>();
            var instance2 = _container.Resolve<TaskBookDbContext>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<TaskBookDbContext>(instance1);
            Assert.IsInstanceOf<TaskBookDbContext>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_TbDataReader_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<TbDataReader>();
            var instance2 = _container.Resolve<TbDataReader>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<TbDataReader>(instance1);
            Assert.IsInstanceOf<TbDataReader>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_ReaderRepository_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IReaderRepository>();
            var instance2 = _container.Resolve<IReaderRepository>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<ReaderRepository>(instance1);
            Assert.IsInstanceOf<ReaderRepository>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_ProjectRepository_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IProjectRepository>();
            var instance2 = _container.Resolve<IProjectRepository>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<ProjectRepository>(instance1);
            Assert.IsInstanceOf<ProjectRepository>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_ProjectUsersRepository_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IProjectUsersRepository>();
            var instance2 = _container.Resolve<IProjectUsersRepository>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<ProjectUsersRepository>(instance1);
            Assert.IsInstanceOf<ProjectUsersRepository>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_TaskRepository_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<ITaskRepository>();
            var instance2 = _container.Resolve<ITaskRepository>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<TaskRepository>(instance1);
            Assert.IsInstanceOf<TaskRepository>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_ProjectAccessService_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IProjectAccessService>();
            var instance2 = _container.Resolve<IProjectAccessService>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<ProjectAccessService>(instance1);
            Assert.IsInstanceOf<ProjectAccessService>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_UserService_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IUserService>();
            var instance2 = _container.Resolve<IUserService>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<UserService>(instance1);
            Assert.IsInstanceOf<UserService>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_ProjectService_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IProjectService>();
            var instance2 = _container.Resolve<IProjectService>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<ProjectService>(instance1);
            Assert.IsInstanceOf<ProjectService>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_PermissionService_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IPermissionService>();
            var instance2 = _container.Resolve<IPermissionService>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<PermissionService>(instance1);
            Assert.IsInstanceOf<PermissionService>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_RoleService_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IRoleService>();
            var instance2 = _container.Resolve<IRoleService>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<RoleService>(instance1);
            Assert.IsInstanceOf<RoleService>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_TaskService_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<ITaskService>();
            var instance2 = _container.Resolve<ITaskService>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<TaskService>(instance1);
            Assert.IsInstanceOf<TaskService>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_EmailService_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IEmailService>();
            var instance2 = _container.Resolve<IEmailService>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<EmailService>(instance1);
            Assert.IsInstanceOf<EmailService>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void UnityTest_UnitOfWork_Resolving()
        {
            // Arrange, Act
            var instance1 = _container.Resolve<IUnitOfWork>();
            var instance2 = _container.Resolve<IUnitOfWork>();

            // Assert
            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.IsInstanceOf<UnitOfWork>(instance1);
            Assert.IsInstanceOf<UnitOfWork>(instance2);
            Assert.AreNotSame(instance1, instance2);
        }
    }
}
