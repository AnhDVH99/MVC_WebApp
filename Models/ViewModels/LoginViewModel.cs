using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Please enter user name!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter password!")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }

    }
}
