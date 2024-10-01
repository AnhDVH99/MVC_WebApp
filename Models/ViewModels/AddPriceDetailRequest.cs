using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class AddPriceDetailRequest
    {
        public Guid PriceDetailID { get; set; }

        [ForeignKey("Product")]
        public Guid ProductID { get; set; }

        public Guid PriceID { get; set; }

        public int VAT { get; set; }

        public float EnvirontmentTax { get; set; }

        public Guid UnitID { get; set; }

        public Unit? Unit { get; set; }

        public Product? Product { get; set; }

        public Price? Price { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> Units { get; set; }
    }
}
