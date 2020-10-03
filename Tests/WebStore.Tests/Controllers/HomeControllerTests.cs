using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebStore.Controllers;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_View() 
        {
            var controller = new HomeController();
            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Blog_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.Blogs();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void BlogSingle_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.BlogSingle();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.ContactUs();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void Throw_Throw_ApplicationException()
        {
            var controller = new HomeController();

            const string id = "test";

            var result = controller.Throw(id);
        }

        [TestMethod]
        public void Throw_Throw_ApplicationException2()
        {
            var controller = new HomeController();

            const string id = "test";
            var expected_message = $"Исключение: {id}";

            var exception = Assert.Throws<ApplicationException>(() => controller.Throw(id));

            var actual_message = exception.Message;

            Assert.Equal(expected_message, actual_message);
        }

        [TestMethod]
        public void Error404_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.Error404();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ErrorStatus_404_RedirectTo_Error404()
        {
            var controller = new HomeController();
            const string code = "404";
            var result = controller.ErrorStatus(code);

            var actual_result = Assert.IsType<RedirectToActionResult>(result);

            const string expected_method_name = (nameof(HomeController.Error404));

            Assert.Equal(expected_method_name, actual_result.ActionName);
            Assert.Null(actual_result.ControllerName);
        }

    }
}
