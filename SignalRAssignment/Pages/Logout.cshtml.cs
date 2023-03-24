using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Hubs;
using SignalRAssignment.Common;

namespace SignalRAssignment.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IHubContext<FoodStoreHub> foodStoreHub;

        public LogoutModel(IHubContext<FoodStoreHub> foodStoreHub)
        {
            this.foodStoreHub = foodStoreHub;
        }

        public async void OnGet()
        {
            // remove the account from the session
            VaSession.Remove(HttpContext.Session, "Account");
            
            System.Threading.Thread.Sleep(1000);
            await foodStoreHub.Clients.All.SendAsync("LoadIndex");

            Response.Redirect("/Index");
        }
    }
}
