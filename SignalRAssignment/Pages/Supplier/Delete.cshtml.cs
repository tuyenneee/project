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

namespace SignalRAssignment.Pages_Supplier
{
    [StaffPermission]
    public class DeleteModel : PageModel
    {
        private readonly SignalRAssignment.Models.PizzaStoreContext _context;
        private readonly IHubContext<FoodStoreHub> foodStoreHub;
        public DeleteModel(SignalRAssignment.Models.PizzaStoreContext context, IHubContext<FoodStoreHub> foodStoreHub)
        {
            _context = context;
            this.foodStoreHub = foodStoreHub;
        }

        [BindProperty]
        public Supplier Supplier { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(m => m.SupplierId == id);

            if (supplier == null)
            {
                return NotFound();
            }
            else
            {
                Supplier = supplier;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier != null)
            {
                try
                {
                    Supplier = supplier;
                    _context.Suppliers.Remove(Supplier);
                    await _context.SaveChangesAsync();

                    await foodStoreHub.Clients.All.SendAsync("LoadSupplier");
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
                            "Cannot delete supplier because it is referenced by a product."
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
