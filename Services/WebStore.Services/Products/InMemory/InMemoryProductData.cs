using WebStore.Interfaces.Services;
using WebStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using WebStore.Services.Data;

namespace WebStore.Services.Products.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            var query = TestData.Products;

            if (filter?.SectionId != null)
                query = query.Where(product => product.SectionId == filter.SectionId);

            if (filter?.BrandId != null)
                query = query.Where(product => product.BrandId == filter.BrandId);

            return query;
        }

        public Product GetProductById(int id) => TestData.Products.FirstOrDefault(p => p.Id == id);

    }
}
