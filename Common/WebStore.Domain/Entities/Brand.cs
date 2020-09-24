using WebStore.Domain.Entities.Base.Interfaces;
using WebStore.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebStore.Domain.Entities
{
    [Table("ProductBrand")]
    public class Brand : NamedEntity, IOrderedEntity
    {
        public int Order { get ; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
