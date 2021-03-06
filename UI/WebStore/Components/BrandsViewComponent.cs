﻿using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke(string brandId) =>
            View(new SelectableBrandsViewModel
            {
                Brands = GetBrands(),
                CurrentBrandId = int.TryParse(brandId, out var id) ? id : (int?)null
            });

        private IEnumerable<BrandViewModel> GetBrands() =>
            _ProductData.GetBrands()
               .Select(brand => new BrandViewModel
               {
                   Id = brand.Id,
                   Name = brand.Name,
                   Order = brand.Order,
                   ProductsNumber = brand.ProductsNumber
               })
               .OrderBy(brand => brand.Order);

    }
}
