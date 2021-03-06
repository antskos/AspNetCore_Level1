﻿using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.ViewModels
{
    public class CartViewModel
    {
        //public Dictionary<ProductViewModel, int> Items { get; set;  } = new Dictionary<ProductViewModel, int>();
        public IEnumerable<(ProductViewModel product, int quantity)> Items { get; set; }

        public int ItemsCount => Items?.Sum(item => item.quantity) ?? 0;
    }
}
