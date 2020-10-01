using WebStore.Interfaces.Services;
using WebStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using WebStore.Services.Data;
using WebStore.Domain.DTO.Products;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<BrandDTO> GetBrands() => TestData.Brands.Select(b => b.ToDTO());

        public IEnumerable<SectionDTO> GetSections() => TestData.Sections.Select(s => s.ToDTO());

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null)
        {
            var query = TestData.Products;

            if (filter?.SectionId != null)
                query = query.Where(product => product.SectionId == filter.SectionId);

            if (filter?.BrandId != null)
                query = query.Where(product => product.BrandId == filter.BrandId);

            return query.ToDTO();
        }

        public ProductDTO GetProductById(int id) => TestData.Products.FirstOrDefault(p => p.Id == id).ToDTO();

    }
}
