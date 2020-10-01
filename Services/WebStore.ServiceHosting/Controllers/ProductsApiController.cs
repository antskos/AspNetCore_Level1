using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<ProductDTO> GetProducts([FromBody] ProductFilter filter = null)
        {
            return _productData.GetProducts(filter);
        }

        [HttpGet("sections")]
        public IEnumerable<SectionDTO> GetSections()
        {
            return _productData.GetSections();
        }
    }
}
