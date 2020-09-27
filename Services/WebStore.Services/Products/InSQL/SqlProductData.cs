using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebStore.Domain.DTO.Products;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;
        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<SectionDTO> GetSections() => _db.Sections.ToDTO();

        public IEnumerable<BrandDTO> GetBrands() => _db.Brands.ToDTO();

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (filter?.Ids?.Length > 0)
                query = query.Where(pr => filter.Ids.Contains(pr.Id));
            else
            {
                if (filter?.BrandId != null)
                    query = query.Where(pr => pr.BrandId == filter.BrandId);

                if (filter?.SectionId != null)
                    query = query.Where(pr => pr.SectionId == filter.SectionId);
            }

            return query.ToDTO();
        }

        public ProductDTO GetProductById(int id) => _db.Products.
                                                 Include(p => p.Section).
                                                 Include(p => p.Brand).
                                                 FirstOrDefault(p => p.Id == id).
                                                 ToDTO();
    }
}
