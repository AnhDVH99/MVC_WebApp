using Microsoft.AspNetCore.Identity;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class User
{
    public Guid UserID { get; set; }
    
    public string UserName { get; set; }

    public string Password { get; set; }

    public Guid EmployeeID { get; set; }

    public Employee Employee { get; set; }

}