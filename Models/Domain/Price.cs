using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class Price
{
    [Required]
    public Guid PriceID { get; set; }
    
    public string PriceCode { get; set; }

    public DateTime FromDate { get; set; }
    
    public DateTime ToDate { get; set; }
    
    public string SysU { get; set; }

    public DateTime SysD { get; set; }

    public ICollection<PriceDetail>? PriceDetails { get; set; }


}