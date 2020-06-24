using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Contetxt;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;
        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<Section> GetSections() => _db.Sections;

        public IEnumerable<Brand> GetBrands() => _db.Brands;
        
        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
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

            return query;
        }

        public Product GetProductById(int id) => _db.Products.
                                                 Include(p => p.Section).
                                                 Include(p => p.Brand).
                                                 FirstOrDefault(p => p.Id == id);
    }
}
