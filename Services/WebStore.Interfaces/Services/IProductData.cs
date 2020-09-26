using System.Collections.Generic;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<SectionDTO> GetSections();

        IEnumerable<BrandDTO> GetBrands();

        IEnumerable<ProductDTO> GetProducts(ProductFilter filter = null);

        ProductDTO GetProductById(int id);

    }
}
