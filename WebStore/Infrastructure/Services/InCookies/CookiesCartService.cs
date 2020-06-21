using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Services.InCookies
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
            var item = cart.Items.
        }

        public void DecrementFromCart(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCart(int id)
        {
            throw new NotImplementedException();
        }

        public CartViewModel TransformFromCart()
        {
            throw new NotImplementedException();
        }
    }
}
