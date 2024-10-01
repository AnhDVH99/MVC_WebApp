using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Unit>> GetAllAsync();

    }
}
