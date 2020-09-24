using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService) { _cartService = cartService; }

        public IActionResult Details() => 
            View(new CartOrderViewModel 
            {
                Cart = _cartService.TransformFromCart(),
                Order = new OrderViewModel()
            });

        public IActionResult AddToCart(int id) 
        {
            _cartService.AddToCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveAll(int id)
        {
            _cartService.RemoveAll();
            return RedirectToAction(nameof(Details));
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderViewModel model, [FromServices] IOrderService orderService) 
        {
            if(!ModelState.IsValid)
                return View(nameof(Details), new CartOrderViewModel
                {
                    Cart = _cartService.TransformFromCart(),
                    Order = model
                });
            else 
            {
                var order = await orderService.CreateOrder(User.Identity.Name, _cartService.TransformFromCart(), model);

                _cartService.RemoveAll();

                return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id });
            }
        }

        public IActionResult OrderConfirmed(int id) 
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
