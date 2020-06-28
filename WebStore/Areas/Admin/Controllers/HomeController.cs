using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Areas.Admin.Controllers
{
    // контролерам в областях надо в атрибуте указывать принадлежность к области, для исключения неоднозначности
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
