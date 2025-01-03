using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class CreditLimit
{
    public Guid CreditLimitID { get; set; } 

    [Required]
    [ForeignKey("Customer")]
    public Guid CustomerID { get; set; }

    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

    public string? CreditType { get; set; }

    [Required]
    public decimal? Total { get; set; }

    public string? OverDue { get; set; }

    public Customer? Customer { get; set; }
}