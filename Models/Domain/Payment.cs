using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class Payment
{
    public Guid PaymentID { get; set; }

    [Required]
    public string PaymentCode { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal Amount { get; set; }

    [Required]
    
    public Guid CustomerID { get; set; }

    public string PaymentStatus { get; set; }

    public Customer? Customer { get; set; }

}