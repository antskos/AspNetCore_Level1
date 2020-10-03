using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WebStore.Controllers;
using WebStore.Interfaces.TestAPI;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiTestControllerTests
    {
        [TestMethod]
        public void Index_Returns_View_with_Values()
        {
            var expected_values = new[] { "1", "2", "3" };
            
            var value_service_mock = new Mock<IValueService>();

            value_service_mock
                .Setup(service => service.Get())
                .Returns(expected_values);

            var controller = new WebApiTestController(value_service_mock.Object);
            
            var result = controller.Index();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view.Model);

            Assert.Equal(expected_values.Length, model.Count());
            // если объект притворяется интерфейсом, то это называют "Стаб" (Stub) -- часть кода выше коммента

            value_service_mock.Verify(service => service.Get());
            value_service_mock.VerifyNoOtherCalls();

            // Если выполняется последующая проверка состояния объекта: того какие методы вызывались, то это "Мок" (Mock) 
        }
    }
}
