using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public RegisterModel(PizzaStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Account Account { get; set; } = default!;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Accounts == null || Account == null)
            {
                return Page();
            }
            if(Account.Password != Account.ConfirmPassword)
            {
                ModelState.AddModelError("Account.Password", "Passwords do not match");
                return Page();
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserName == Account.UserName);
            if(account != null)
            {
                ModelState.AddModelError("Account.Username", "Username already exists");
                return Page();
            }
            Account.Type = 0;
            _context.Accounts.Add(Account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
