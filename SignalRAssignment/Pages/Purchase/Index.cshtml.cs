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
    [MemberPermission]
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var account = VaSession.Get<Models.Account>(HttpContext.Session, "Account");
            if (_context.Orders != null)
            {
                Order = await _context.Orders
                    .Include(i => i.OrderDetails)
                    .Where(x=>x.AccountId== account.AccountId)
                    .ToListAsync();
            }
        }
    }
   
   

    
}
