using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Infrastructure.Abstract;
using GameStore.WebUI.Models;

namespace GameStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            // створення імітації постачальника аутентифікації
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "12345")).Returns(true);

            // створення моделі подання з правильними обліковими даними
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "12345"
            };

            // Контроллер
            AccountController target = new AccountController(mock.Object);

            // Аутентифікація
            ActionResult result = target.Login(model, "/MyURL");

            // Твердження
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            //створення імітації постачальника аутентифікації
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);

            // створення моделі подання з неправильними обліковими даними
            LoginViewModel model = new LoginViewModel
            {
                UserName = "badUser",
                Password = "badPass"
            };

            // Контролер
            AccountController target = new AccountController(mock.Object);

            // Аутентифікація
            ActionResult result = target.Login(model, "/MyURL");

            // Твердження
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}