using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeShop.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItem> CartItems { get; set; }
        public int? OrderID { get; set; }
        public decimal? TotalCost { get; set; }
    }
}