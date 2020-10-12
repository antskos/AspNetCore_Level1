using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BreadCrumbsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbViewModel();

            if (int.TryParse(Request.Query["SectionId"], out var section_id))
                model.Section = _productData.GetSectionById(section_id).FromDTO();

            if (int.TryParse(Request.Query["BrandId"], out var brand_id))
                model.Brand = _productData.GetBrandById(brand_id).FromDTO();

            if (int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var product_id))
            {
                var product = _productData.GetProductById(product_id);
                if (product != null)
                    model.Product = product.Name;
            }

            return View(model);
        }
    }
}
