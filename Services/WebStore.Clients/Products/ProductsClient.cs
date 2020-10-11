using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {

        public ProductsClient(IConfiguration configuration) : base(configuration, WebAPI.Products) { }

        public BrandDTO GetBrandById(int id)
        {
            return Get<BrandDTO>($"{_serviceAddress}/brands/id");
        }

        public IEnumerable<BrandDTO> GetBrands() => Get<IEnumerable<BrandDTO>>($"{_serviceAddress}/brands");
        
        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_serviceAddress}/{id}");
        
        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null) =>
            Post(_serviceAddress, filter ?? new ProductFilter())
            .Content
            .ReadAsAsync<IEnumerable<ProductDTO>>()
            .Result;

        public SectionDTO GetSectionById(int id)
        {
            return Get<SectionDTO>($"{_serviceAddress}/sections/id");
        }

        public IEnumerable<SectionDTO> GetSections() => Get<IEnumerable<SectionDTO>>($"{_serviceAddress}/sections");

    }
}
