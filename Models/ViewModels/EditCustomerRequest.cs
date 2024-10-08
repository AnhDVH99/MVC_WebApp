using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ASP.NET_Core_MVC_Piacom.Validators;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels;

public class EditCustomerRequest
{
    [Required]
    public Guid CustomerID { get; set; }

    [Required(ErrorMessage = "Please enter your name!")]
    public string CustomerName { get; set; }

    [Required(ErrorMessage = "Please enter your phone number!")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Please enter your address!")]
    public string Address1 { get; set; }
    public string? Address2 { get; set; }

    [Required(ErrorMessage = "Please enter your city!")]
    public string City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }

    [Required]
    public Guid SaleRepEmployeeID { get; set; }

    public List<CreditLimit>? CreditLimits { get; set; } 
    public IEnumerable<SelectListItem>? Employees { get; set; }

}