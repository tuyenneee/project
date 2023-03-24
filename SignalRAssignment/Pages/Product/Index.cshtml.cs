using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Product
{

    [StaffPermission]
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; } = default!;

        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public const int ITEMS_PAGE = 5;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public Func<int?, string> generateUrl { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    try
                    {
                        var value = Decimal.Parse(SearchString);
                        Product = await _context.Products
                            .Include(p => p.Category)
                            .Include(p => p.Supplier)
                            .Where(p => p.UnitPrice == value || p.ProductId == (int)value)
                            .ToListAsync();
                    }
                    catch (Exception)
                    {
                        Product = await _context.Products
                            .Include(p => p.Category)
                            .Include(p => p.Supplier)
                            .Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim()))
                            .ToListAsync();
                    }

                }
                else
                {
                    Product = await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Supplier)
                        .ToListAsync();

                    int total = await _context.Products.CountAsync();
                    countPages = (int)Math.Ceiling((double)total / ITEMS_PAGE);
                    if (currentPage < 1)
                    {
                        currentPage = 1;
                    }
                    Product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                     .Skip(ITEMS_PAGE * (currentPage - 1))
                     .Take(ITEMS_PAGE)
                    .ToListAsync();
                }
            }
        }
    }
}
