using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Profile
{
    public class EditModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public EditModel(PizzaStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (Account.UserName == null || Account.Password == null || Account.FullName == null)
            {
                return Page();
            }

            _context.Attach(Account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(Account.AccountId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
