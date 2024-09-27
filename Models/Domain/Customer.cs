using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class Customer
{
    public Guid CustomerID { get; set; }

    [Required(ErrorMessage ="Please enter your name!")]
    public string CustomerName { get; set; }

    [Required(ErrorMessage ="Please enter your phone!")]
    public string Phone { get; set; }

    [Required(ErrorMessage ="Please enter your address!")]
    public string AddressLine1 { get; set; }
    
    public string? AddressLine2 { get; set; }

    [Required(ErrorMessage = "Please enter a city! ")]    
    public string City { get; set; }
    
    public string? State { get; set; }
    
    public string? PostalCode { get; set; }
    
    public string? Country { get; set; }


    public Guid SaleRepEmployeeID { get; set; }

    public ICollection<Payment>? Payments { get; set; }

    public ICollection<CreditLimit>? CreditLimits { get; set; }

    public ICollection<Order>? Orders { get; set; }
    
}