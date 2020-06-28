using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.DAL.Contetxt;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

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

        public Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<Order> GetOrderById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}
