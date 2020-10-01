using Microsoft.Extensions.DependencyInjection;
using WebStore.Interfaces.Services;
using WebStore.Services.Products.InCookies;
using WebStore.Services.Products.InSQL;

namespace WebStore.Services.Products
{
    public static class SerrvicesExtensions
    {
        public static IServiceCollection AddWebStoreServices(this IServiceCollection services)
        {
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<IEmployeesData, SqlEmployeesData>();
            services.AddScoped<ICartService, CookiesCartService>();
            services.AddScoped<IOrderService, SqlOrderService>();

            return services;
        }
    }
}
