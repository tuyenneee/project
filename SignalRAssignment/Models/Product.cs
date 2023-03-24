using System;
using System.Collections.Generic;

namespace SignalRAssignment.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string ProductName { get; set; } = null!;
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductImage { get; set; }
        public int ProductId { get; set; }
        public string? Description { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
