using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Oba30.Controllers;
using Oba30.Models;
using Oba30.Providers;
using Rhino.Mocks;

namespace Oba30.Tests
{
    [TestFixture]
    public class AdminControllerTests
    {
        private AdminController _adminController;
        private IAuthProvider _authProvider;

        [SetUp]
        public void Setup()
        {
            _authProvider = MockRepository.GenerateMock<IAuthProvider>();
            _adminController = new AdminController(_authProvider);

            var httpContextMock = MockRepository.GenerateMock<HttpContextBase>();
            _adminController.Url = new UrlHelper(new RequestContext(httpContextMock, new RouteData()));

        }

        [Test(Description = "Test that user is logged in")]
        public void Login_IsLogged_True_Test()
        {
            // arrange
            _authProvider.Stub(s => s.IsLoggiedIn).Return(true);
            
            // act
            var actual = _adminController.Login("/admin/manage");

            Assert.IsInstanceOf<RedirectResult>(actual);
            Assert.AreEqual("/admin/manage", ((RedirectResult)actual).Url);
        }

        [Test(Description = "Test for invalid user login")]
        public void Login_Post_Model_Invalid_Test()
        {
            // arrange
            var model = new LoginModel();
            _adminController.ModelState.AddModelError("UserName", "UserName is required");

            // act
            var actual = _adminController.Login(model, "/");

            // assert
            Assert.IsInstanceOf<ViewResult>(actual);
        }

        [Test(Description = "Test for invalid user credentials.")]
        public void Login_Post_User_Invalid_Test()
        {
            // arrange
            var model = new LoginModel()
            {
                UserName = "invaliduser",
                Password = "password"
            };

            _authProvider.Stub(s => s.Login(model.UserName, model.Password))
                .Return(false);

            // act
            var actual = _adminController.Login(model, "/");

            // assert
            Assert.IsInstanceOf<ViewResult>(actual);
            var modelStateErrors = _adminController.ModelState[""].Errors;
            Assert.IsTrue(modelStateErrors.Count > 0);
            Assert.AreEqual("The user name or password provided is incorrect.", modelStateErrors[0].ErrorMessage);
        }

        [Test(Description = "Test for valid user credntials")]
        public void Login_Post_User_Valid_Test()
        {
            // arrange
            var model = new LoginModel()
            {
                UserName = "validuser",
                Password = "password"
            };

            _authProvider.Stub(s => s.Login(model.UserName, model.Password))
                .Return(true);

            // act
            var actual = _adminController.Login(model, "/");

            // assert
            Assert.IsInstanceOf<RedirectResult>(actual);
            Assert.AreEqual("/", ((RedirectResult)actual).Url);
        }
    }
}
