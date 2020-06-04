using WebStore.Infrastructure.Interfaces;
using WebStore.Domain.Entities;
using System.Collections.Generic;
using WebStore.Data;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;
    }
}
