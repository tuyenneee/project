using System;
using System.Collections.Generic;

namespace SignalRAssignment.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public string CompanyName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int SupplierId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
