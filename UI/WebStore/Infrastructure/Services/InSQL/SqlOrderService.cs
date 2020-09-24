using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Transactions;
using WebStore.DAL.Contetxt;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services.InSQL
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

        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) 
                throw new InvalidOperationException($"Пользователь {userName} не найден в БД");

            await using var tr = await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                Name = orderModel.Name,
                Address = orderModel.Address,
                Phone = orderModel.Phone,
                User = user,
                Date = DateTime.Now,
                Items = new List<OrderItem>()
            };

            foreach (var (product_model, quantity) in cart.Items)
            {
                var product = await _db.Products.FindAsync(product_model.Id);

                if (product is null) 
                    throw new InvalidOperationException($"Товар id:{product.Id} не найден в БД");

                var order_item = new OrderItem
                {
                    Product = product,
                    Order = order,
                    Quantity = quantity,
                    Price = product.Price
                };
                order.Items.Add(order_item);
            }

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            await tr.CommitAsync();

            return order;
        }

        public async Task<Order> GetOrderById(int id) => 
            await _db.Orders.
            Include(o => o.Items).
            FirstOrDefaultAsync(o => o.Id == id);


        public async Task<IEnumerable<Order>> GetUserOrders(string userName) => 
            await _db.Orders.
            Include(o => o.User).
            Include(o => o.Items).
            Where(o => o.User.UserName == userName).
            ToArrayAsync();  
        
    }
}
