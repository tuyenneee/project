using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using SignalRAssignment.Hubs;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Supplier
{
    [StaffPermission]
    public class CreateModel : PageModel
    {
        private readonly SignalRAssignment.Models.PizzaStoreContext _context;
        private readonly IHubContext<FoodStoreHub> foodStoreHub;

        public CreateModel(SignalRAssignment.Models.PizzaStoreContext context, IHubContext<FoodStoreHub> foodStoreHub)
        {
            _context = context;
            this.foodStoreHub = foodStoreHub;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Supplier Supplier { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Suppliers == null || Supplier == null)
            {
                return Page();
            }

            _context.Suppliers.Add(Supplier);
            await _context.SaveChangesAsync();

            await foodStoreHub.Clients.All.SendAsync("LoadSupplier");

            return RedirectToPage("./Index");
        }
    }
}
