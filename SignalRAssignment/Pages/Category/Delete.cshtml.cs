using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Hubs;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Category
{
    [StaffPermission]
    public class DeleteModel : PageModel
    {
        private readonly PizzaStoreContext _context;
        private readonly IHubContext<FoodStoreHub> foodStoreHub;
        public DeleteModel(PizzaStoreContext context, IHubContext<FoodStoreHub> foodStoreHub)
        {
            _context = context;
            this.foodStoreHub = foodStoreHub;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                try
                {
                    Category = category;
                    _context.Categories.Remove(Category);
                    await _context.SaveChangesAsync();

                    await foodStoreHub.Clients.All.SendAsync("LoadCategory");
                }
                catch (Exception e)
                {
                    if (
                        e.InnerException != null
                        && e.InnerException.Message.Contains(
                            "The DELETE statement conflicted with the REFERENCE constraint"
                        )
                    )
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            "Cannot delete product because it is referenced by other products."
                        );
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, e.Message);
                    }
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
