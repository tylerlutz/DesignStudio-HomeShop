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
    public partial class CustomerOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerOrder()
        {
            this.ShoppingCartItems = new HashSet<ShoppingCartItem>();
        }
    
        public int OrderID { get; set; }
        public string CustomerID { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<int> ShippingID { get; set; }
        public string TransactionID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
        public virtual ShippingType ShippingType { get; set; }

        public string Token { get; set; }
    }
}
