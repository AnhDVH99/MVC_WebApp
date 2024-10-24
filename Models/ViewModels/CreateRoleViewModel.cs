using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }

        [Display(Name = "Permissions")]
        public List<string> Permissions { get; set; } = new List<string>();
    }
}
