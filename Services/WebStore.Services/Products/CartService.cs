using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products
{
    public class CartService : ICartService
    {
        private readonly IProductData _productData;
        private readonly ICartStore _cartStore;

        public CartService(IProductData productData, ICartStore cartStore)
        {
            _productData = productData;
            _cartStore = cartStore;
        }

        public void AddToCart(int id)
        {
            var cart = _cartStore.Cart;
            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem() { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;
            // сериализация корзины в cookies(которая, является строчкой json) 
            _cartStore.Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            // мой код
            var cart = _cartStore.Cart;
            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);

            if (item.Quantity == 1)
                cart.Items.Remove(item);
            else
                item.Quantity--;
            _cartStore.Cart = cart;

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
            var cart = _cartStore.Cart;
            cart.Items.Clear();         // я написал RemoveAll -- надо будет посмотреть различия
            _cartStore.Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = _cartStore.Cart;
            var item = cart.Items.FirstOrDefault(item => item.ProductId == id);

            if (item is null)
                return;
            else
                cart.Items.Remove(item);

            _cartStore.Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = _cartStore.Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var product_view_models = products.FromDTO().ToView().ToDictionary(pr => pr.Id);

            return new CartViewModel
            {
                Items = _cartStore.Cart.Items.Select(item => (product_view_models[item.ProductId], item.Quantity))
            };
        }
    }
}
