using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route(WebAPI.Products)]    // [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductData
    {
        private readonly IProductData _productData;

        public ProductsApiController(IProductData productData)
        {
            _productData = productData;
        }

        [HttpGet("brands/{id}")]
        public BrandDTO GetBrandById(int id)
        {
            return _productData.GetBrandById(id);
        }

        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands()
        {
            return _productData.GetBrands();
        }

        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id)
        {
            return _productData.GetProductById(id);
        }

        [HttpPost]
        public PageProductsDTO GetProducts([FromBody] ProductFilter filter = null)
        {
            return _productData.GetProducts(filter);
        }

        [HttpGet("sections/{id}")]
        public SectionDTO GetSectionById(int id)
        {
            return _productData.GetSectionById(id);
        }

        [HttpGet("sections")]
        public IEnumerable<SectionDTO> GetSections()
        {
            return _productData.GetSections();
        }
    }
}
