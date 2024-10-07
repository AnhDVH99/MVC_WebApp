using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class PriceDetail
{
    [Required(ErrorMessage ="Required")]
    public Guid PriceDetailID { get; set; }

    [Required(ErrorMessage = "Required")]
    public Guid ProductID { get; set; }

    [Required(ErrorMessage = "Required")]
    public Guid PriceID { get; set; }

    public float PriceBeforeTax { get; set; }
    public float VAT { get; set; }
    
    public float EnvirontmentTax { get; set; }

    public float Price { get; set; }

    [Required(ErrorMessage = "Required")]
    public Guid UnitID { get; set; }

    public virtual Product? Product { get; set; }  // Navigation property
    public virtual Unit? Unit { get; set; }

    public virtual Price? PriceNav { get; set; }
}