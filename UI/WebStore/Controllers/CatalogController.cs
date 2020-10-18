using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
            var page_size = int.TryParse(_configuration["PageSize"], out var size) ? size : (int?)null;
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
                            .OrderBy(p => p.Order)
            });
        }

        public IActionResult Details(int id) 
        {
            var product = _productData.GetProductById(id);

            if (product is null)
                return NotFound();
            else return View(product.FromDTO().ToView());
        }

    }
}
