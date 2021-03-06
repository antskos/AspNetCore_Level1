﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Domain.DTO.Products;
using WebStore.Services.Mapping;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        public CatalogController(IProductData productData)
        {
            _productData = productData;
        }
        public IActionResult Index() => View(_productData.GetProducts().Products.FromDTO());

        public IActionResult Edit(int? id) 
        {
            var product = id is null ? new ProductDTO() : _productData.GetProductById((int)id);

            if (product is null)
                return NotFound();
            else
                return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Product product) 
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id) 
        {
            var product = _productData.GetProductById(id);

            if (product is null)
                return NotFound();
            else
                return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName(nameof(Delete))]
        public IActionResult DeleteConfirm(int id) => RedirectToAction(nameof(Index));
    }
}
