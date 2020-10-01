using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Contetxt;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _userManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<OrderDTO> CreateOrder(string userName, CreateOrderModel orderModel)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                throw new InvalidOperationException($"Пользователь {userName} не найден в БД");

            await using var tr = await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                Name = orderModel.Order.Name,
                Address = orderModel.Order.Address,
                Phone = orderModel.Order.Phone,
                User = user,
                Date = DateTime.Now,
                Items = new List<OrderItem>()
            };

            foreach (var item in orderModel.Items)
            {
                var product = await _db.Products.FindAsync(item.Id);

                if (product is null)
                    throw new InvalidOperationException($"Товар id:{product.Id} не найден в БД");

                var order_item = new OrderItem
                {
                    Product = product,
                    Order = order,
                    Quantity = item.Quantity,
                    Price = product.Price
                };
                order.Items.Add(order_item);
            }

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            await tr.CommitAsync();

            return order.ToDTO();
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order =  await _db.Orders.
                               Include(o => o.User).
                               FirstOrDefaultAsync(o => o.Id == id);
            return order.ToDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string userName)
        {
            var orders = await _db.Orders.
                               Include(o => o.User).
                               Include(o => o.Items).
                               Where(o => o.User.UserName == userName).
                               ToArrayAsync();
            return orders.Select(o => o.ToDTO());
        }
    }
}
