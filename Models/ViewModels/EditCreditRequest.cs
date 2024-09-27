using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class EditCreditRequest
    {
        public Guid CreditLimitID { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public Guid CustomerID { get; set; }

        [Required(ErrorMessage ="Please select Date!")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Please select Date!")]
        public string ToDate { get; set; }

        public string? CreditType { get; set; }

        public decimal? Total { get; set; }

        public string? OverDue { get; set; }

        public Customer? Customer { get; set; }

        public IEnumerable<SelectListItem>? Customers { get; set; }
    }
}
