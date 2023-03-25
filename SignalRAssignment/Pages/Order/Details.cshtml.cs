using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Order
{
    [StaffPermission]
    public class DetailsModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public DetailsModel(PizzaStoreContext context)
        {
            _context = context;
        }
        public string selectedOption = string.Empty;
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
                OrderDetail = await _context.OrderDetails.Where(o => o.OrderId == id).Include(o => o.Product).ToListAsync();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string selectedOption, int id)
        {
            Console.WriteLine(selectedOption);
            var order = await _context.Orders.Where(o => o.OrderId == id).FirstOrDefaultAsync();
            order.Status = Int16.Parse(selectedOption);
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }

    }
}
