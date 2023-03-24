using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRAssignment.Models
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
        }

        public int AccountId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
 

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; } = null!;
        public short Type { get; set; }
    }
}
