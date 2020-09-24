using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InCookies
{
    public class CookiesCartService : ICartService
    {
        private readonly IProductData _productData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cartName;

        public Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cartCookie = context.Request.Cookies[_cartName];

                if (cartCookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                else
                {
                    ReplaceCookie(cookies, cartCookie);
                    return JsonConvert.DeserializeObject<Cart>(cartCookie);
                }
            }
            set
            {
                ReplaceCookie(_httpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
            }
        }

        private void ReplaceCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cookie);
        }

        public CookiesCartService(IProductData productData, IHttpContextAccessor httpContextAccessor)
        {
            _productData = productData;
            _httpContextAccessor = httpContextAccessor;

            var user = httpContextAccessor.HttpContext.User;
            var userName = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            _cartName = $"WebStore.Cart[{userName}]";

        }

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem() { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;
            // сериализация корзины в cookies(которая, является строчкой json) 
            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            // мой код
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);

            if (item.Quantity == 1)
                cart.Items.Remove(item);
            else
                item.Quantity--;
            Cart = cart;

            // у преподавателя такой код
            //if (item is null) return;   // не понятно как такое может быть(по идее к не добавленому товару, мы не должны иметь возможности применить этот метод)
            //if (item.Quantity > 0)
            //    item.Quantity--;
            //if (item.Quantity == 0)
            //    cart.Items.Remove(item);
            // и вообще может вынести во ViewModel, что или товара одна штука или больше, или он удаляется из списка ???
        }

        public void RemoveAll()
        {
            var cart = Cart;
            cart.Items.Clear();         // я написал RemoveAll -- надо будет посмотреть различия
            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);

            if (item is null)
                return;
            else
                cart.Items.Remove(item);

            Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var product_view_models = products.ToView().ToDictionary(pr => pr.Id);

            return new CartViewModel
            {
                Items = Cart.Items.Select(item => (product_view_models[item.ProductId], item.Quantity))
            };
        }
    }
}
