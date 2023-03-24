using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Purchase
{
    public class OrderDetailsModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public OrderDetailsModel(PizzaStoreContext context)
        {
            _context = context;
        }

      public Order Order { get; set; } = default!; 

      public List<OrderDetail> OrderDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o => o.Account).FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            else 
            {
                Order = order;
                OrderDetail = await _context.OrderDetails
                    .Where(o => o.OrderId == id)
                    .Include(o => o.Product)
                    .ToListAsync();
            }
            return Page();
        }
    }
}
