﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Products;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Products
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _Cart;

        private Mock<IProductData> _ProductDataMock;
        private Mock<ICartStore> _CartStoreMock;

        /// <summary>Тестируемый сервис</summary>
        private ICartService _CartService;

        [TestInitialize]
        public void TestInitialize()
        {
            _Cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 3 }
                }
            };

            _ProductDataMock = new Mock<IProductData>();

            _ProductDataMock
               .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
               .Returns(new PageProductsDTO
               {

                   Products = new List<ProductDTO>
                    {
                        new ProductDTO
                        {
                            Id = 1,
                            Name = "Product 1",
                            Price = 1.1m,
                            Order = 0,
                            ImageUrl = "Product1.png",
                            Brand = new BrandDTO { Id = 1, Name = "Brand 1" },
                            Section = new SectionDTO { Id = 1, Name = "Section 1"}
                        },
                        new ProductDTO
                        {
                            Id = 2,
                            Name = "Product 2",
                            Price = 2.2m,
                            Order = 0,
                            ImageUrl = "Product2.png",
                            Brand = new BrandDTO { Id = 2, Name = "Brand 2" },
                            Section = new SectionDTO { Id = 2, Name = "Section 2"}
                        },
                    },
                   TotalCount = 2
               });
            _CartStoreMock = new Mock<ICartStore>();
            _CartStoreMock.Setup(c => c.Cart).Returns(_Cart);
            _CartService = new CartService(_ProductDataMock.Object, _CartStoreMock.Object);
        }

        // тест на то, что модель корзины правильно считает колчиство товаров
        [TestMethod]
        public void CartModel_ItemsCount_returns_Correct_Quantity()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 3 },
                }
            };

            const int expected_count = 4;

            var actual_count = cart.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }

        // тест на правильность подсчёта общего количества товаров корзины для модели представления
        [TestMethod]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            var cart_view_model = new CartViewModel
            {
                Items = new[]
                {
                    ( new ProductViewModel { Id = 1, Name = "Product 1", Price = 5m }, 1 ),
                    ( new ProductViewModel { Id = 2, Name = "Product 2", Price = 15m }, 3 ),
                }
            };

            const int expected_count = 4;

            var actual_count = cart_view_model.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }

        [TestMethod]
        public void CartService_AddToCart_WorkCorrect()
        {
            _Cart.Items.Clear();

            const int expected_id = 5;

            _CartService.AddToCart(expected_id);

            Assert.Equal(1, _Cart.ItemsCount);

            Assert.Single(_Cart.Items);
            Assert.Equal(expected_id, _Cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartService_RemoveFromCart_Remove_Correct_Item()
        {
            const int item_id = 1;

            _CartService.RemoveFromCart(item_id);

            Assert.Single(_Cart.Items);
            Assert.Equal(2, _Cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartService_Clear_ClearCart()
        {
            _CartService.RemoveAll();

            Assert.Empty(_Cart.Items);
        }

        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            const int item_id = 2;

            _CartService.DecrementFromCart(item_id);

            Assert.Equal(3, _Cart.ItemsCount);      // общее кол-во товаров стало после декремента
            Assert.Equal(2, _Cart.Items.Count);
            var items = _Cart.Items.ToArray();
            Assert.Equal(item_id, items[1].ProductId);
            Assert.Equal(2, items[1].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int item_id = 1;

            _CartService.DecrementFromCart(item_id);

            Assert.Equal(3, _Cart.ItemsCount);
            Assert.Single(_Cart.Items);
        }

        [TestMethod]
        public void CartService_TransformFromCart_WorkCorrect()
        {
            var result = _CartService.TransformFromCart();

            Assert.Equal(4, result.ItemsCount);
            Assert.Equal(1.1m, result.Items.First().product.Price);
        }

    }
}
