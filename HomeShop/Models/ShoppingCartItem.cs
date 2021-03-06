//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HomeShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class ShoppingCartItem
    {
        public int CartID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Nullable<decimal> Price { get; set; }
    
        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
