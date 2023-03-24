using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Profile
{
    [MemberPermission]
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var account = VaSession.Get<Account>(HttpContext.Session, "Account");
            var accountFull = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountId == account.AccountId);
            if (accountFull == null)
            {
                return NotFound();
            }
            else
            {
                Account = accountFull;
            }
            return Page();
        }
    }
}
