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

namespace SignalRAssignment.Pages_Product
{
  
  [StaffPermission]
    public class CreateModel : PageModel
    {
        private readonly PizzaStoreContext _context;
        private readonly IHubContext<FoodStoreHub> foodStoreHub;
        public CreateModel(PizzaStoreContext context, IHubContext<FoodStoreHub> foodStoreHub)
        {
            _context = context;
            this.foodStoreHub = foodStoreHub;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Product.ProductName))
            {
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            await foodStoreHub.Clients.All.SendAsync("LoadProduct");

            return RedirectToPage("./Index");
        }
    }
}
