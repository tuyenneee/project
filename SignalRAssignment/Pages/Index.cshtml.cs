using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;
namespace SignalRAssignment.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Models.Product> Product { get; set; } = default!;
        public IList<Models.Category> Categories { get; set; } = default!;

        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public decimal? priceFrom { get; set; }
        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public decimal? priceTo { get; set; }
        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public int? cateId { get; set; }
        public const int ITEMS_PAGE = 6;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public Func<int?, string> generateUrl { get; set; }
        public async Task OnGetAsync()
        {
            ////Xu ly category
            if(_context.Categories != null)
            {
                Categories = await _context.Categories.ToListAsync();
            }

            if(cateId != null)
            {
                int total = await _context.Products
                    .Where(p => p.CategoryId == cateId)
                    .CountAsync();
                countPages = (int)Math.Ceiling((double)total / ITEMS_PAGE);
                if (currentPage < 1)
                {
                    currentPage = 1;
                }
                Product = await _context.Products
                       .Include(p => p.Category)
                       .Include(p => p.Supplier)
                       .Where(p => p.CategoryId == cateId)
                       .Skip(ITEMS_PAGE * (currentPage - 1))
                       .Take(ITEMS_PAGE)
                       .ToListAsync();
                return;
            }


            if (_context.Products != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    try
                    {
                        var valueSearch = decimal.Parse(SearchString);

                        //Search theo price+ FromTo
                        if (priceFrom != null && priceTo != null)
                        {
                            Product = await _context.Products
                                  .Include(p => p.Category)
                                  .Include(p => p.Supplier)
                                  .Where(p => p.UnitPrice == valueSearch 
                                    && p.UnitPrice >= priceFrom && p.UnitPrice <= priceTo)
                                  .ToListAsync();
                        }
                        else
                        {
                            //Search theo Price
                            Product = await _context.Products
                                  .Include(p => p.Category)
                                  .Include(p => p.Supplier)
                                  .Where(p => p.UnitPrice == valueSearch)
                                  .ToListAsync();
                        }

                    }
                    catch (Exception)
                    {
                        if (priceFrom != null && priceTo != null)
                        {
                            int total = await _context.Products
                                 .Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim())
                                        && p.UnitPrice >= priceFrom && p.UnitPrice <= priceTo)
                                .CountAsync();
                            countPages = (int)Math.Ceiling((double)total / ITEMS_PAGE);
                            if (currentPage < 1)
                            {
                                currentPage = 1;
                            }
                            //Search theo Product Name + Price
                            Product = await _context.Products
                                   .Include(p => p.Category)
                                   .Include(p => p.Supplier)
                                   .Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim())
                                        && p.UnitPrice >= priceFrom && p.UnitPrice <= priceTo)
                                    .Skip(ITEMS_PAGE * (currentPage - 1))
                                    .Take(ITEMS_PAGE)
                                   .ToListAsync();
                        }
                        else
                        {
                            //Search theo Product Name
                            int total = await _context.Products
                                .Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim()))
                                .CountAsync();
                            countPages = (int)Math.Ceiling((double)total / ITEMS_PAGE);
                            if (currentPage < 1)
                            {
                                currentPage = 1;
                            }
                            Product = await _context.Products
                                   .Include(p => p.Category)
                                   .Include(p => p.Supplier)
                                   .Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim()))
                                    .Skip(ITEMS_PAGE * (currentPage - 1))
                                    .Take(ITEMS_PAGE)
                                   .ToListAsync();
                        }
                        
                    }
                }
                else
                {
                    int total = await _context.Products.CountAsync();
                    countPages = (int)Math.Ceiling((double)total / ITEMS_PAGE);
                    if(currentPage <1)
                    {
                        currentPage = 1;
                    }

                    Product = await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Supplier)
                        .Skip(ITEMS_PAGE * (currentPage-1))
                        .Take(ITEMS_PAGE)
                        .ToListAsync();
                }
            }
        }

    }
}
