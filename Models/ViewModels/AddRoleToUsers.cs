namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class AddRoleToUsers
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }
    }
}
