using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class EditOrderRequest
    {
        [Required]
        public Guid OrderID { get; set; }

        [Required(ErrorMessage ="Please enter order date!")]
        public string OrderDate { get; set; }

        [Required(ErrorMessage = "Please enter order date!")]
        public string RequiredDate { get; set; }

        [Required(ErrorMessage = "Please enter order date!")]
        public string ShippedDate { get; set; }

        [Required(ErrorMessage = "Please enter order date!")]
        public string? OrderStatus { get; set; }

        public string? Comment { get; set; }

        [ForeignKey("Customer")]

        [Required]
        public Guid CustomerID { get; set; }

        [Required]
        public Guid EmployeeID { get; set; }

        public string SysU { get; set; }

        public DateTime SysD { get; set; }

        public Customer Customer { get; set; } 
        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

    }
}
