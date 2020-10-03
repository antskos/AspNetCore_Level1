using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Blogs() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult Cart() => View();

        public IActionResult CheckOut() => View();

        public IActionResult ContactUs() => View();

        public IActionResult Login() => View();

        public IActionResult ProductDetails() => View();

        public IActionResult Shop() => View();

        public IActionResult Throw(string id) =>
            throw new ApplicationException($"Исключение: {id ?? "без id"}");

        public IActionResult Error404() => View();

        public IActionResult ErrorStatus(string code)
        {
            switch (code)
            {
                case "404":
                    return RedirectToAction(nameof(Error404));

                default:
                    return Content($"Error {code}");
            }
        }
    }
}
