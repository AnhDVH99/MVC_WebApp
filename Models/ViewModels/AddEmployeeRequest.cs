using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class AddEmployeeRequest
    {
        [Required]
        public Guid EmployeeID { get; set; }

        [Required(ErrorMessage = "Please enter name!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter name!")]
        public string LastName { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number!")]
        public string Phone { get; set; }

        [Required(ErrorMessage ="Please enter job title")]
        public string JobTitle { get; set; }

    }
}
