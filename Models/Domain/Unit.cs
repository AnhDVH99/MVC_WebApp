using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class Unit
{
    [Required]
    public Guid UnitID { get; set; }

    [Required(ErrorMessage = "Please enter Unit code")]
    public string UnitCode { get; set; }

    [Required(ErrorMessage = "Please enter Unit name")]
    public string UnitName { get; set; }


}