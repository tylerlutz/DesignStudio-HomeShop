using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HomeShop.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItem> CartItems { get; set; }
        public int? OrderID { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal? TotalCost { get; set; }
    }
}