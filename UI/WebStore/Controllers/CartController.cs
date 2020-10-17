using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Domain.DTO.Orders;

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

        public IActionResult RemoveAll()
        {
            _cartService.RemoveAll();
            return RedirectToAction(nameof(Details));
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new CartOrderViewModel
                {
                    Cart = _cartService.TransformFromCart(),
                    Order = OrderModel
                });

            var orderModel = new CreateOrderModel
            {
                Order = OrderModel,
                Items = _cartService.TransformFromCart().Items
                   .Select(item => new OrderItemDTO
                   {
                       Id = item.product.Id,
                       Price = item.product.Price,
                       Quantity = item.quantity
                   })
            };

            var order = await OrderService.CreateOrder(User.Identity.Name, orderModel);

            _cartService.RemoveAll();

            return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id) 
        {
            ViewBag.OrderId = id;
            return View();
        }

        #region API

        public IActionResult GetCartView() => ViewComponent("Cart");

        [HttpGet("add/{id}")]
        [HttpGet("increment/{id}")]
        public IActionResult AddToCartAPI(int id)
        {
            _cartService.AddToCart(id);
            return Json(new { id, message = $"Товар с id:{id} добавлен в корзину" });
        }

        [HttpGet("decrement/{id}")]
        public IActionResult DecrementFromCartAPI(int id)
        {
            _cartService.DecrementFromCart(id);
            return Ok();

        }

        [HttpGet("remove/{id}")]
        public IActionResult RemoveFromCartAPI(int id)
        {
            _cartService.RemoveFromCart(id);
            return Ok();
        }

        [HttpGet("remove/all")]
        public IActionResult RemoveAllAPI()
        {
            _cartService.RemoveAll();
            return Ok();
        }

        #endregion

    }
}
