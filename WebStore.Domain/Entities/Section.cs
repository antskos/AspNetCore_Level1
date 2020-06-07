using WebStore.Domain.Entities.Base.Interfaces;
using WebStore.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace WebStore.Domain.Entities
{
    [Table("ProductSection")]
    public class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int? ParentId { get; set; }

        public virtual Section ParentSection { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
