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
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;
        public const int ITEMS_PAGE = 5;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public Func<int?, string> generateUrl { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                int total = await _context.Orders.CountAsync();
                countPages = (int)Math.Ceiling((double)total / ITEMS_PAGE);
                if (currentPage < 1)
                {
                    currentPage = 1;
                }
                Order = await _context.Orders
                .Include(o => o.Account)
                 .Skip(ITEMS_PAGE * (currentPage - 1))
                  .Take(ITEMS_PAGE)
                .ToListAsync();
            }
        }
    }
}
