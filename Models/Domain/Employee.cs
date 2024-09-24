using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class Employee
{
    [Required]
    public Guid EmployeeID { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string? Email { get; set; }
    
    [Required]
    public string Phone { get; set; }

    [Required]
    public string JobTitle { get; set; }

    public User User { get; set; }

    public ICollection<Customer> Customers { get; set; }

}