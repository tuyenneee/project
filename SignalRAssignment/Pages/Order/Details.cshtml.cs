using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;
using ClosedXML.Excel;

namespace SignalRAssignment.Pages_Order
{
    [StaffPermission]
    public class DetailsModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public DetailsModel(PizzaStoreContext context)
        {
            _context = context;
        }

      public Order Order { get; set; } = default!; 

      public List<OrderDetail> OrderDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o => o.Account).FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            else 
            {
                Order = order;
                OrderDetail = await _context.OrderDetails.Where(o => o.OrderId == id).Include(o => o.Product).ToListAsync();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id, string hander)
        {
            if (hander.Equals("print"))
            {
                Console.WriteLine("print");
                await savePDF(id);
            }

            var order = await _context.Orders.Include(o => o.Account).FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
                OrderDetail = await _context.OrderDetails.Where(o => o.OrderId == id).Include(o => o.Product).ToListAsync();
            }
            return Page();
        }
        public async Task savePDF(int id)
        {
            int row = 8;
             Order = await _context.Orders.Include(o => o.Account).FirstOrDefaultAsync(m => m.OrderId == id);
            OrderDetail = await _context.OrderDetails.Where(o => o.OrderId == id).Include(o => o.Product).ToListAsync();
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("details");
            ws.Cell("A1").Value = "ORDER";
            ws.Cell("A2").Value = "OrderDate";
            ws.Cell("B2").Value = Order.OrderDate;
            ws.Cell("A3").Value = "ShippedDate";
            ws.Cell("B3").Value = Order.ShippedDate;
            ws.Cell("A4").Value = "Freight";
            ws.Cell("B4").Value = Order.Freight;
            ws.Cell("A5").Value = "ShipAddress";
            ws.Cell("B5").Value = Order.ShipAddress;
            ws.Cell("A6").Value = "ORDER DETAILS";
            ws.Cell("A7").Value = "ProductName";
            ws.Cell("B7").Value = "quantity";
            ws.Cell("C7").Value = "unitPrice";
            ws.Cell("D7").Value = "Total";
            foreach (var entity in OrderDetail)
            {
                ws.Cell("A" +row).Value = entity.Product.ProductName;
                ws.Cell("B" +row).Value = entity.Quantity;
                ws.Cell("C" +row).Value = entity.UnitPrice;
                ws.Cell("D" +row).Value = entity.Quantity * entity.UnitPrice;
                row++;
                Console.WriteLine(row);
            }
            string nameFile = "Export_" + DateTime.Now.Ticks + ".xlsx";
            string pathFile = "d:/XuatFileExcel/" + nameFile;
            wb.SaveAs(pathFile);
        }
       
    }
}
    