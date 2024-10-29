namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class EditRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public List<string> SelectedPermissions { get; set; } = new List<string>();

        public List<string> AvailablePermissions { get; set; } = new List<string>();

    }
}
