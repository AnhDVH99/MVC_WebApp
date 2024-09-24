using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class PriceDetail
{
    [Required(ErrorMessage ="Required")]
    public Guid PriceDetailID { get; set; }

    [Required(ErrorMessage = "Required")]
    [ForeignKey("Product")]
    public Guid ProductID { get; set; }

    [Required(ErrorMessage = "Required")]
    public Guid PriceID { get; set; }

    public int VAT { get; set; }
    
    public float EnvirontmentTax { get; set; }

    [Required(ErrorMessage = "Required")]
    public Guid UnitID { get; set; }

    public Unit? Unit { get; set; }

    public Product? Product { get; set; }    

    public Price? Price { get; set; }
}