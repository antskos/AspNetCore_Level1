using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Mapping;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Shop(int? sectionId, int? brandId)
        {
            var filter = new ProductFilter
            {
                SectionId = sectionId,
                BrandId = brandId
            };

            var products = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                Products = products.ToView().OrderBy(p => p.Order)
            });
        }
    }
}
