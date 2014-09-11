using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.AuthManagers;
using TaskBook.Services.Interfaces;
using TaskBook.WebApi.Controllers;
using NLog.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace TaskBook.Test
{
    [TestFixture]
    public sealed class AccountControllerTest
    {
        private TbUserManager _userManager;
        private Mock<ILogger> _logger;
        private Mock<IUserService> _userService;


        [SetUp]
        public void Init()
        {
            var userStore = new Mock<IUserStore<TbUser>>();
            _userManager = new TbUserManager(userStore.Object);
            _logger = new Mock<ILogger>();
            _userService = new Mock<IUserService>();
        }

        [TearDown]
        public void Cleanup()
        {

        }

        [Test]
        public void AccountController_GetUserByUserName_Ok()
        {
            // Arrange
            var user = new TbUserRoleVm()
            {
                UserName = "User1"
            };

            _userService.Setup(x => x.GetUserByUserName(user.UserName))
                .Returns(user)
                .Verifiable("Must call IUserService.GetUserByUserName");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.GetUserByUserName(user.UserName) as OkNegotiatedContentResult<TbUserRoleVm>;

            // Assert
            Assert.AreEqual(user.UserName, result.Content.UserName);
            Assert.IsInstanceOf<OkNegotiatedContentResult<TbUserRoleVm>>(result);
            _userService.Verify();
        }

        [Test]
        public void AccountController_GetUserByUserName_Bad()
        {
            // Arrange
            TbUserRoleVm user = null;
            string userName = "User1";

            _userService.Setup(x => x.GetUserByUserName(It.IsAny<string>()))
               .Returns(user)
               .Verifiable("Must call IUserService.GetUserByUserName");

            _logger.Setup(x => x.Warning(It.IsAny<string>()))
               .Verifiable("Must call ILogger.Warning");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.GetUserByUserName(userName) as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
            _userService.Verify();
            _logger.Verify();
        }

        [Test]
        public void AccountController_GetUsersWithRolesByProjectId_Ok()
        {
            // Arrange
            long progectId = 1;
            var users = new List<TbUserRoleVm>()
            {
                new TbUserRoleVm()
                {
                    ProjectId = progectId,
                    UserName = "User1"
                },
                new TbUserRoleVm()
                {
                    ProjectId = progectId,
                    UserName = "User2"
                }
            };

            _userService.Setup(x => x.GetUsersWithRolesByProjectId(progectId))
                .Returns(users.AsQueryable())
                .Verifiable("Must call IUserService.GetUsersWithRolesByProjectId");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.GetUsersWithRolesByProjectId(progectId) as OkNegotiatedContentResult<IQueryable<TbUserRoleVm>>;

            // Assert
            Assert.AreEqual(users.Count(), result.Content.Count());
            Assert.AreEqual(users.ElementAt(0).ProjectId, result.Content.ElementAt(0).ProjectId);
            Assert.AreEqual(users.ElementAt(0).UserName, result.Content.ElementAt(0).UserName);
            Assert.IsInstanceOf<OkNegotiatedContentResult<IQueryable<TbUserRoleVm>>>(result);
            _userService.Verify();
        }

        [Test]
        public void AccountController_GetUsersWithRolesByProjectId_Bad()
        {
            // Arrange
            long progectId = 1;
            IQueryable<TbUserRoleVm> users = null;

            _userService.Setup(x => x.GetUsersWithRolesByProjectId(progectId))
                .Returns(users)
                .Verifiable("Must call IUserService.GetUsersWithRolesByProjectId");

            _logger.Setup(x => x.Warning(It.IsAny<string>()))
               .Verifiable("Must call ILogger.Warning");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.GetUsersWithRolesByProjectId(progectId) as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
            _userService.Verify();
            _logger.Verify();
        }

        [Test]
        public void AccountController_GetUsersByProjectId_Ok()
        {
            // Arrange
            long progectId = 1;
            var users = new List<UserProjectVm>()
            {
                new UserProjectVm()
                {
                    ProjectId = progectId,
                    UserName = "User1"
                },
                new UserProjectVm()
                {
                    ProjectId = progectId,
                    UserName = "User2"
                }
            };

            _userService.Setup(x => x.GetUsersByProjectId(progectId))
               .Returns(users.AsQueryable())
               .Verifiable("Must call IUserService.GetUsersByProjectId");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.GetUsersByProjectId(progectId) as OkNegotiatedContentResult<IQueryable<UserProjectVm>>;

            // Assert
            Assert.AreEqual(users.Count(), result.Content.Count());
            Assert.AreEqual(users.ElementAt(0).ProjectId, result.Content.ElementAt(0).ProjectId);
            Assert.AreEqual(users.ElementAt(0).UserName, result.Content.ElementAt(0).UserName);
            Assert.IsInstanceOf<OkNegotiatedContentResult<IQueryable<UserProjectVm>>>(result);
            _userService.Verify();
        }

        [Test]
        public void AccountController_GetUsersByProjectId_Bad()
        {
            // Arrange
            long progectId = 1;
            IQueryable<UserProjectVm> users = null;

            _userService.Setup(x => x.GetUsersByProjectId(progectId))
               .Returns(users)
               .Verifiable("Must call IUserService.GetUsersByProjectId");

            _logger.Setup(x => x.Warning(It.IsAny<string>()))
               .Verifiable("Must call ILogger.Warning");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.GetUsersByProjectId(progectId) as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
            _userService.Verify();
            _logger.Verify();
        }
    }
}
