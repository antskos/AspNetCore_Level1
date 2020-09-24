using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        /* если нужны проекции классов в нескольких местах,
         * то экземпляр AutoMapper инициализируем
           в конструкторе для всего класса */
        //private readonly IMapper _mapper;

        public CatalogController(IProductData ProductData, IMapper mapper)
        {
            _productData = ProductData;
            //_mapper = mapper;
        }

        // можно подключить экземпляр AutoMapper через атрибут, если он нужен только в одном из методов
        public IActionResult Shop(int? sectionId, int? brandId, [FromServices] IMapper mapper)
        {
            var filter = new ProductFilter
            {
                SectionId = sectionId,
                BrandId = brandId
            };

            var products = _productData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                Products = products.
                            //ToView()      //замена ручной проекции на автоматическую класса AutoMapper
                            //Select(p => mapper.Map<ProductViewModel>(p)). // вызывается так
                            Select(mapper.Map<ProductViewModel>).           // или так
                            OrderBy(p => p.Order)
            });
        }

        public IActionResult Details(int id) 
        {
            var product = _productData.GetProductById(id);

            if (product is null)
                return NotFound();
            else return View(product.ToView());
        }

    }
}
