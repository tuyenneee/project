using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRAssignment.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public double Freight { get; set; }
        public string ShipAddress { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        
        
    }
}
