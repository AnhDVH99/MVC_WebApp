using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();

        Task<Employee?> GetAsync(Guid id);

        Task<Employee> AddAsync(Employee employee);

        Task<Employee?> UpdateAsync(Employee employee);

        Task<Employee?> DeleteAsync(Guid id);
    }
}
