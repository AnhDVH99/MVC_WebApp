using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class Unit
{
    [Required]
    public Guid UnitID { get; set; }
    
    public string UnitCode { get; set; }

    public string UnitName { get; set; }


}