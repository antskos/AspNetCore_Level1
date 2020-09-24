using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class CartViewModel
    {
        //public Dictionary<ProductViewModel, int> Items { get; set;  } = new Dictionary<ProductViewModel, int>();
        public IEnumerable<(ProductViewModel product, int quantity)> Items { get; set; }

        public int ItemsCount => Items?.Sum(item => item.quantity) ?? 0;
    }
}
