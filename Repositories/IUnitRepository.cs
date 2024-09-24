using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Unit>> GetAllAsync();

        Task<Unit?> GetAsync(Guid id);

        Task<Unit> AddAsync(Unit unit);

        Task<Unit?> UpdateAsync(Unit unit);

        Task<Unit?> DeleteAsync(Guid id);

    }
}
