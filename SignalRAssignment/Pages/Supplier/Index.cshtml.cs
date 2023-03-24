using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Supplier
{
    [StaffPermission]
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Supplier> Supplier { get; set; } = default!;
        public const int ITEMS_PAGE = 5;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public Func<int?, string> generateUrl { get; set; }

        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.Suppliers != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    try
                    {
                        var value = Decimal.Parse(SearchString);
                        Supplier = await _context.Suppliers
                            .Where(s => s.SupplierId == (int)value)
                            .ToListAsync();
                    }
                    catch (Exception)
                    {
                        Supplier = await _context.Suppliers
                            .Where(s => s.CompanyName.ToLower().Contains(SearchString.ToLower().Trim()) ||
                            s.Address.ToLower().Contains(SearchString.ToLower().Trim()))
                            .ToListAsync();
                    }

                }
                else
                {
                    Supplier = await _context.Suppliers
                        .ToListAsync();

                    int total = await _context.Suppliers.CountAsync();
                    countPages = (int)Math.Ceiling((double)total / ITEMS_PAGE);
                    if (currentPage < 1)
                    {
                        currentPage = 1;
                    }
                    Supplier = await _context.Suppliers
                         .Skip(ITEMS_PAGE * (currentPage - 1))
                         .Take(ITEMS_PAGE)
                        .ToListAsync();
                }
            }
        }
    }
}
