using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Hubs;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Account
{
    public class EditModel : PageModel
    {
        private readonly SignalRAssignment.Models.PizzaStoreContext _context;
        private readonly IHubContext<FoodStoreHub> foodStoreHub;
        public EditModel(SignalRAssignment.Models.PizzaStoreContext context, IHubContext<FoodStoreHub> foodStoreHub)
        {
            _context = context;
            this.foodStoreHub = foodStoreHub;   
        }

        [BindProperty]
        public Models.Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account =  await _context.Accounts.FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }
            Account = account;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (Account.UserName == null || Account.Password==null || Account.FullName == null)
            {
                return Page();
            }

            _context.Attach(Account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                await foodStoreHub.Clients.All.SendAsync("LoadAccount");
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
