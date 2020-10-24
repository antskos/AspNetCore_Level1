using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly IConfiguration _configuration;
        private const string _pageSize = "PageSize";

        /* если нужны проекции классов в нескольких местах,
         * то экземпляр AutoMapper инициализируем
           в конструкторе для всего класса */
        //private readonly IMapper _mapper;

        public CatalogController(IProductData ProductData, IConfiguration configuration)
        {
            _productData = ProductData;
            _configuration = configuration;
            //_mapper = mapper;
        }

        // можно подключить экземпляр AutoMapper через атрибут, если он нужен только в одном из методов
        public IActionResult Shop(int? sectionId, int? brandId, int page = 1)
        {
            var page_size = int.TryParse(_configuration[_pageSize], out var size) ? size : (int?)null;
            var filter = new ProductFilter
            {
                SectionId = sectionId,
                BrandId = brandId,
                Page = page,
                PageSize = page_size
            };

            var products = _productData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                Products = products.Products
                            .FromDTO()
                            .ToView()
                            .OrderBy(p => p.Order),
                PageViewModel = new PageViewModel 
                {
                    PageSize = page_size ?? 0,
                    PageNumber = page,
                    TotalItems = products.TotalCount
                }
            });
        }

        public IActionResult Details(int id) 
        {
            var product = _productData.GetProductById(id);

            if (product is null)
                return NotFound();
            else return View(product.FromDTO().ToView());
        }


        #region API

        public IActionResult GetCatalogHtml(int? sectionId, int? brandId, int page)
        {
            return PartialView("Partial/_FeaturesItems", GetProducts(sectionId, brandId, page));
        }

        public IEnumerable<ProductViewModel> GetProducts(int? sectionId, int? brandId, in int page = 1)
        {
            return _productData.GetProducts(new ProductFilter
            {
                SectionId = sectionId,
                BrandId = brandId,
                Page = page,
                PageSize = int.Parse(_configuration[_pageSize])
            }).Products
                .FromDTO()
                .ToView()
                .OrderBy(p => p.Order);
                             
        }

        #endregion
    }
}
