using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface ICreditRepository
    {
        Task<IEnumerable<CreditLimit>> GetAllAsync();

        Task<CreditLimit?> GetAsync(Guid id);

        Task<CreditLimit> AddAsync(CreditLimit creditLimit);

        Task<CreditLimit?> UpdateAsync(CreditLimit creditLimit);

        Task<CreditLimit?> DeleteAsync(Guid id);

    }
}
