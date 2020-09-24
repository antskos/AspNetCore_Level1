﻿using System.Collections.Generic;
using System.Linq;
using WebStore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke() => View(GetBrands());

        private IEnumerable<BrandViewModel> GetBrands() =>
            _ProductData.GetBrands()
               .Select(brand => new BrandViewModel
               {
                   Id = brand.Id,
                   Name = brand.Name,
                   Order = brand.Order
               })
               .OrderBy(brand => brand.Order);

    }
}