using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface IPriceRepository
    {
        Task<IEnumerable<Price>> GetAllAsync();

        Task<Price?> GetAsync(Guid id);

        Task<Price> AddAsync(Price price);

        Task<Price?> UpdateAsync(Price price);

        Task<Price?> DeleteAsync(Guid id);

    }
}
