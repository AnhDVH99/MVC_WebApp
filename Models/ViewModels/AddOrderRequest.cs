using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class AddOrderRequest
    {
        [Required]
        public Guid OrderID { get; set; }

        [Required(ErrorMessage = "Please enter order date!")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter required date!")]
        public DateTime RequiredDate { get; set; } = DateTime.Now.AddDays(7);

        public DateTime? ShippedDate { get; set; } 

        [Required]
        public string OrderStatus { get; set; }

        public string? Comment { get; set; }

        [ForeignKey("Customer")]

        [Required]
        public Guid CustomerID { get; set; }

        [Required]
        public Guid EmployeeID { get; set; }

        public string SysU { get; set; }

        public DateTime SysD { get; set; }

        public Customer Customer { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }


    }
}
