using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        public async Task<Unit> AddAsync(Unit unit)
        {
            throw new NotImplementedException();
        }

        public async Task<Unit?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Unit>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Unit?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Unit?> UpdateAsync(Unit unit)
        {
            throw new NotImplementedException();
        }
    }
}
