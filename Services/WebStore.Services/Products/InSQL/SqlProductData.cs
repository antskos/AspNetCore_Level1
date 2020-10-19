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

        public SectionDTO GetSectionById(int id) => _db.Sections.Find(id).ToDTO();

        public IEnumerable<BrandDTO> GetBrands() => _db.Brands.Include(b => b.Products).ToDTO();

        public BrandDTO GetBrandById(int id) => _db.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == id).ToDTO();

        public PageProductsDTO GetProducts(ProductFilter filter = null)
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

            var total_count = query.Count();

            if (filter?.PageSize > 0)
                query = query
                    .Skip((filter.Page - 1) * (int)filter.PageSize)
                    .Take((int)filter.PageSize);

            return new PageProductsDTO
            {
                Products = query.AsEnumerable().ToDTO(),
                TotalCount = total_count
            };
        }

        public ProductDTO GetProductById(int id) => _db.Products.
                                                 Include(p => p.Section).
                                                 Include(p => p.Brand).
                                                 FirstOrDefault(p => p.Id == id).
                                                 ToDTO();
    }
}
