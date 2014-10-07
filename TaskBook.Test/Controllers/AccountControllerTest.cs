using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Moq;
using NLog.Mvc;
using NUnit.Framework;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DomainModel;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services.Interfaces;
using TaskBook.WebApi.Controllers;

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

            _userService.Setup(x => x.GetUserWithRoleByUserName(user.UserName))
                .Returns(user)
                .Verifiable("Must call IUserService.GetUserByUserName");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.GetUserByUserName(user.UserName) as OkNegotiatedContentResult<TbUserRoleVm>;

            // Assert
            _userService.Verify();
            Assert.IsInstanceOf<OkNegotiatedContentResult<TbUserRoleVm>>(result);
            Assert.AreEqual(user.UserName, result.Content.UserName);
        }

        [Test]
        public void AccountController_GetUserByUserName_Bad()
        {
            // Arrange
            TbUserRoleVm user = null;
            string userName = "User1";

            _userService.Setup(x => x.GetUserWithRoleByUserName(It.IsAny<string>()))
               .Returns(user)
               .Verifiable("Must call IUserService.GetUserByUserName");

            _logger.Setup(x => x.Warning(It.IsAny<string>()))
               .Verifiable("Must call ILogger.Warning");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.GetUserByUserName(userName) as BadRequestErrorMessageResult;

            // Assert
            _userService.Verify();
            _logger.Verify();
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
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
            _userService.Verify();
            Assert.IsInstanceOf<OkNegotiatedContentResult<IQueryable<TbUserRoleVm>>>(result);
            Assert.AreEqual(users.Count(), result.Content.Count());
            Assert.AreEqual(users.ElementAt(0).ProjectId, result.Content.ElementAt(0).ProjectId);
            Assert.AreEqual(users.ElementAt(0).UserName, result.Content.ElementAt(0).UserName);
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
            _userService.Verify();
            _logger.Verify();
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
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
            _userService.Verify();
            Assert.IsInstanceOf<OkNegotiatedContentResult<IQueryable<UserProjectVm>>>(result);
            Assert.AreEqual(users.Count(), result.Content.Count());
            Assert.AreEqual(users.ElementAt(0).ProjectId, result.Content.ElementAt(0).ProjectId);
            Assert.AreEqual(users.ElementAt(0).UserName, result.Content.ElementAt(0).UserName);
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
            _userService.Verify();
            _logger.Verify();
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void AccountController_ForgotPassword_Ok()
        {
            // Arrange
            var model = new ForgotPasswordVm()
            {
                Email = "tb@tb.com"
            };

            _userService.Setup(x => x.ForgotPassword(model))
                .Returns(() => Task.FromResult(model))
                .Verifiable("Must call IUserService.ForgotPassword");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.ForgotPassword(model).Result;

            // Assert
            _userService.Verify(x => x.ForgotPassword(It.Is<ForgotPasswordVm>(m => m.Equals(model))));
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void AccountController_ForgotPassword_Invalid_Model()
        {
            // Arrange
            var model = new ForgotPasswordVm();

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);
            controller.ModelState.Clear();
            controller.ModelState.AddModelError("TB", "Model is invalid");

            // Act
            var result = controller.ForgotPassword(model).Result;

            // Assert
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void AccountController_ResetPassword_Ok()
        {
            // Arrange
            var model = new ResetPasswordVm()
            {
                UserName = "User1"
            };

            _userService.Setup(x => x.ResetPassword(model))
                .Returns(() => Task.FromResult(model))
                .Verifiable("Must call IUserService.ResetPassword");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.ResetPassword(model).Result;

            // Assert
            _userService.Verify(x => x.ResetPassword(It.Is<ResetPasswordVm>(m => m.Equals(model))));
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void AccountController_ResetPassword_Invalid_Model()
        {
            // Arrange
            var model = new ResetPasswordVm();

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);
            controller.ModelState.Clear();
            controller.ModelState.AddModelError("TB", "Model is invalid");

            // Act
            var result = controller.ResetPassword(model).Result;

            // Assert
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void AccountController_AddUserAsync_Ok()
        {
            // Arrange
            var model = new TbUserRoleVm()
            {
                UserName = "User1"
            };

            _userService.Setup(x => x.AddUserAsync(model))
                .Returns(() => Task.FromResult(model))
                .Verifiable("Must call IUserService.AddUserAsync");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.AddUserAsync(model).Result;

            // Assert
            _userService.Verify(x => x.AddUserAsync(It.Is<TbUserRoleVm>(m => m.Equals(model))));
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void AccountController_AddUserAsync_Invalid_Model()
        {
            // Arrange
            var model = new TbUserRoleVm();

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);
            controller.ModelState.Clear();
            controller.ModelState.AddModelError("TB", "Model is invalid");

            // Act
            var result = controller.AddUserAsync(model).Result;

            // Assert
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void AccountController_UpdateUser_Ok()
        {
            // Arrange
            string id = "AAAA-BBBB";
            var model = new TbUserRoleVm()
            {
                UserId = id,
                UserName = "User1"
            };

            _userService.Setup(x => x.UpdateUser(id, model))
                .Verifiable("Must call IUserService.UpdateUser");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.UpdateUser(id, model);

            // Assert
            _userService.Verify();
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void AccountController_UpdateUser_Invalid_Model()
        {
            // Arrange
            string id = "AAAA-BBBB";
            var model = new TbUserRoleVm();

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);
            controller.ModelState.Clear();
            controller.ModelState.AddModelError("TB", "Model is invalid");

            // Act
            var result = controller.UpdateUser(id, model);

            // Assert
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public void AccountController_DeleteUser_Ok()
        {
            // Arrange
            string id = "AAAA-BBBB";

            _userService.Setup(x => x.DeleteUser(id, false))
               .Verifiable("Must call IUserService.DeleteUser");

            var controller = new AccountController(_userService.Object, _userManager, _logger.Object);

            // Act
            var result = controller.DeleteUser(id);

            // Assert
            _userService.Verify();
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}
