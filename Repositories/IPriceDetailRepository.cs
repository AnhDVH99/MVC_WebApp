using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface IPriceDetailRepository
    {
        Task<IEnumerable<PriceDetail>> GetAllAsync();

        Task<PriceDetail?> GetAsync(Guid id);

        Task<PriceDetail> AddAsync(PriceDetail priceDetail);

        Task<PriceDetail?> UpdateAsync(PriceDetail priceDetail);

        Task<PriceDetail?> DeleteAsync(Guid id);

    }
}
