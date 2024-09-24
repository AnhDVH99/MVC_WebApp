using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public class PriceDetailRepository : IPriceDetailRepository
    {
        private readonly PiacomDbContext piacomDbContext;

        public PriceDetailRepository(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;
        }

        public async Task<PriceDetail> AddAsync(PriceDetail priceDetail)
        {
            await piacomDbContext.PriceDetails.AddAsync(priceDetail);
            await piacomDbContext.SaveChangesAsync();
            return priceDetail;
        }

        public async Task<PriceDetail?> DeleteAsync(Guid id)
        {
            var existingPriceDetails = await piacomDbContext.PriceDetails.FindAsync(id);

            if (existingPriceDetails != null)
            {
                piacomDbContext.PriceDetails.Remove(existingPriceDetails);
                await piacomDbContext.SaveChangesAsync();
                return existingPriceDetails;
            }
            return null;
        }

        public async Task<IEnumerable<PriceDetail>> GetAllAsync()
        {
            return await piacomDbContext.PriceDetails
                .Include(pd => pd.Product)
                .Include(pd => pd.Unit)
                .Include(pd => pd.Price)
                .ToListAsync();
        }

        public Task<PriceDetail?> GetAsync(Guid id)
        {
            return piacomDbContext.PriceDetails
                .Include(pd => pd.Product)
                .Include(pd => pd.Unit)
                .Include(pd => pd.Price)
                .FirstOrDefaultAsync(pd => pd.PriceDetailID == id);
        }

        public async Task<PriceDetail?> UpdateAsync(PriceDetail priceDetail)
        {
            return null;
        }
    }
}

