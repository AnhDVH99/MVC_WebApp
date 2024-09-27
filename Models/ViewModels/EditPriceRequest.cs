using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class EditPriceRequest
    {
        public Guid PriceID { get; set; }

        public string PriceCode { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SysU { get; set; }

        public DateTime SysD { get; set; }

        public List<PriceDetail> PriceDetails { get; set; }

        public List<SelectListItem>? Products { get; set; }
    }
}
