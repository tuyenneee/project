using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SignalRAssignment.Pages.Helpers
{
    public class PagingModel
    {
        public int currentpage { get; set; }
        public int countpages { get; set; }
        public Func<int?, string> generateUrl { get; set; }

    }
}
