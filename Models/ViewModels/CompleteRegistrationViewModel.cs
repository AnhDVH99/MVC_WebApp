using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class CompleteRegistrationViewModel
    {
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
