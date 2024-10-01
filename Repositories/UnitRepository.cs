using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly PiacomDbContext piacomDbContext;

        public UnitRepository(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;
        }
        public async Task<IEnumerable<Unit>> GetAllAsync()
        {
            return await piacomDbContext.Units.ToListAsync();
        }
    }
}
