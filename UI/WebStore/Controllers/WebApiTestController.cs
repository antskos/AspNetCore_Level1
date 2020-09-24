using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers
{
    public class WebApiTestController : Controller
    {
        private readonly IValueService _valueService;

        public WebApiTestController(IValueService valueService)
        {
            _valueService = valueService;
        }
        public IActionResult Index()
        {
            var values = _valueService.Get();

            return View(values);
        }
    }
}
