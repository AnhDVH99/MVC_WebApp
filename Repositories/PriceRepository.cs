using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly PiacomDbContext piacomDbContext;

        public PriceRepository(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;
        }

        public async Task<Price> AddAsync(Price price)
        {
            await piacomDbContext.Prices.AddAsync(price);
            await piacomDbContext.SaveChangesAsync();
            return price;
        }

        public async Task<Price?> DeleteAsync(Guid id)
        {
            var existingPrice = await piacomDbContext.Prices.FindAsync(id);
            if (existingPrice != null)
            {
                piacomDbContext.Prices.Remove(existingPrice);
                await piacomDbContext.SaveChangesAsync();
                return existingPrice;
            }
            return null;
        }

        public async Task<IEnumerable<Price>> GetAllAsync()
        {
            return await piacomDbContext.Prices.ToListAsync();
        }

        public Task<Price?> GetAsync(Guid id)
        {
            return piacomDbContext.Prices.Include(c => c.PriceDetails).FirstOrDefaultAsync(x => x.PriceID == id);
        }


        public async Task<Price?> UpdateAsync(Price price)
        {
            var existingPrice = await piacomDbContext.Prices
                .Include(c => c.PriceDetails)
                .FirstOrDefaultAsync(c => c.PriceID == price.PriceID);
            var existingProduct = await piacomDbContext.Products
                .AnyAsync(p => p.ProductID == price.PriceID);
                
            if (existingPrice != null)
            {
                //Update customer entity
                piacomDbContext.Entry(existingPrice).CurrentValues.SetValues(price);

                var updatedPriceDetails = price.PriceDetails ?? new List<PriceDetail>();

                var newPriceDetails = updatedPriceDetails
                    .Where(updatePD => updatePD.PriceDetailID == Guid.Empty)
                    .ToList();

                var existingPriceDetailsToUpdate = updatedPriceDetails
                    .Where(updatePD => existingPrice.PriceDetails
                    .Any(existingPD => existingPD.PriceDetailID == updatePD.PriceDetailID))
                    .ToList();


                var lstNotExistPriceDetail = new List<PriceDetail>();
                if (lstNotExistPriceDetail.Count > 0)
                    existingPrice.PriceDetails.AddRange(lstNotExistPriceDetail);

                if (newPriceDetails.Count > 0)
                {
                    foreach (var newPriceDetail in newPriceDetails)
                    {
                        newPriceDetail.PriceID = price.PriceID;
                        if (!await piacomDbContext.Products.AnyAsync(p => p.ProductID == newPriceDetail.ProductID))
                        {
                            throw new InvalidOperationException($"ProductID {newPriceDetail.ProductID} does not exist.");
                        }
                        newPriceDetail.UnitID = newPriceDetail.UnitID;
                    }
                    existingPrice.PriceDetails.AddRange(newPriceDetails);

                }

                if (existingPriceDetailsToUpdate.Count > 0)
                {
                    foreach (var existingPriceDetails in existingPriceDetailsToUpdate)
                    {
                        var dbPriceDetail = existingPrice.PriceDetails
                            .FirstOrDefault(pd => pd.PriceID == existingPriceDetails.PriceID);

                        if (dbPriceDetail != null)
                        {
                            dbPriceDetail.UnitID = existingPriceDetails.UnitID;
                            dbPriceDetail.ProductID = existingPriceDetails.ProductID;
                            dbPriceDetail.VAT = existingPriceDetails.VAT;
                            dbPriceDetail.EnvirontmentTax = existingPriceDetails.EnvirontmentTax;
                        }
                    }
                }

                var priceDetailsToRemove = existingPrice.PriceDetails
                     .Where(existingPD => !updatedPriceDetails
                         .Any(updatedPD => updatedPD.PriceDetailID == existingPD.PriceDetailID))
                     .ToList();
                if (priceDetailsToRemove.Count > 0)
                {
                    piacomDbContext.PriceDetails.RemoveRange(priceDetailsToRemove);
                }

                await piacomDbContext.SaveChangesAsync();
                return existingPrice;
            }
            return null;
        }
    }
}

