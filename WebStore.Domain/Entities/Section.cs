using WebStore.Domain.Entities.Base.Interfaces;
using WebStore.Domain.Entities.Base;


namespace WebStore.Domain.Entities
{
    public class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int? ParentId { get; set; }
    }
}
