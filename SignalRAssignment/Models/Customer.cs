using System;
using System.Collections.Generic;

namespace SignalRAssignment.Models
{
    public partial class Customer
    {
        public string Password { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
