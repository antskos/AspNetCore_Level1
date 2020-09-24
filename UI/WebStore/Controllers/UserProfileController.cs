using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();


        public async Task<IActionResult> Orders([FromServices] IOrderService orderService) 
        {
            var orders = await orderService.GetUserOrders(User.Identity.Name);

            return View(orders.Select(o =>
                new UserOrderViewModel
                {
                    Name = o.Name,
                    Address = o.Address,
                    Id = o.Id,
                    Phone = o.Phone,
                    TotalSum = o.Items.Sum(item => item.Price * item.Quantity)
                }));
        }
    }
}
