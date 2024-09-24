using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product?> GetAsync(Guid id);

        Task<Product> AddAsync(Product product);

        Task<Product?> UpdateAsync(Product product);

        Task<Product?> DeleteAsync(Guid id);

    }
}
