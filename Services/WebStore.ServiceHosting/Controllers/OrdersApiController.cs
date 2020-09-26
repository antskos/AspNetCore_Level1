using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _orderService;

        public OrdersApiController(IOrderService orderService) => _orderService = orderService;

        [HttpGet("user/{userName}")]
        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string userName)
        {
            return await _orderService.GetUserOrders(userName);
        }

        [HttpGet("{id}")]
        public async Task<OrderDTO> GetOrderById(int id)
        {
            return await _orderService.GetOrderById(id);
        }

        [HttpPost("{userName}")]
        public async Task<OrderDTO> CreateOrder(string userName, [FromBody] CreateOrderModel orderModel)
        {
            return await _orderService.CreateOrder(userName, orderModel);
        }
    }
}