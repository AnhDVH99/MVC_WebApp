using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PiacomDbContext piacomDbContext;

        public ProductRepository(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await piacomDbContext.Products.AddAsync(product);
            await piacomDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(Guid id)
        {

            var existingProduct = await piacomDbContext.Products.FindAsync(id);
            var exisitingPriceDetails = await piacomDbContext.PriceDetails.ToListAsync();
            if (existingProduct != null)
            {
                piacomDbContext.Products.Remove(existingProduct);
                await piacomDbContext.SaveChangesAsync();
                return existingProduct;
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await piacomDbContext.Products.ToListAsync();
        }

        public Task<Product?> GetAsync(Guid id)
        {
            return piacomDbContext.Products.Include(c => c.PriceDetails).FirstOrDefaultAsync(x => x.ProductID == id);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await piacomDbContext.Products
            .Include(c => c.PriceDetails) // Include related CreditLimits
            .FirstOrDefaultAsync(c => c.ProductID == id); // Query by CustomerID
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProduct = await piacomDbContext.Products
                .Include(c => c.PriceDetails)
                .FirstOrDefaultAsync(c => c.ProductID == product.ProductID);
            if (existingProduct != null)
            {
                //Update customer entity
                piacomDbContext.Entry(existingProduct).CurrentValues.SetValues(product);

                var updatedPriceDetails = product.PriceDetails ?? new List<PriceDetail>();

                var newPriceDetails = updatedPriceDetails
                    .Where(updatePD => updatePD.PriceDetailID == Guid.Empty)
                    .ToList();

                var existingPriceDetailsToUpdate = updatedPriceDetails
                    .Where(updatePD => existingProduct.PriceDetails
                    .Any(existingPD => existingPD.PriceDetailID == updatePD.PriceDetailID))
                    .ToList();

                var lstNotExistPriceDetail = new List<PriceDetail>();

                var existingUnits = await piacomDbContext.Units.ToListAsync();
                var existingPrices = await piacomDbContext.Prices.ToListAsync();

                if (lstNotExistPriceDetail.Count > 0)
                    existingProduct.PriceDetails.AddRange(lstNotExistPriceDetail);

                if (newPriceDetails.Count > 0)
                {
                    foreach (var newPriceDetail in newPriceDetails)
                    {
                        newPriceDetail.ProductID = product.ProductID;
                        newPriceDetail.PriceID = existingPrices.FirstOrDefault().PriceID; // Example logic, adjust as needed
                        newPriceDetail.UnitID = existingUnits.FirstOrDefault().UnitID;
                    }
                    existingProduct.PriceDetails.AddRange(newPriceDetails);
                }

                if (existingPriceDetailsToUpdate.Count > 0)
                {
                    foreach (var existingPriceDetails in existingPriceDetailsToUpdate)
                    {
                        var dbPriceDetail = existingProduct.PriceDetails
                            .FirstOrDefault(pd => pd.PriceID == existingPriceDetails.PriceID);

                        if (dbPriceDetail != null)
                        {
                            dbPriceDetail.VAT = existingPriceDetails.VAT;
                            dbPriceDetail.EnvirontmentTax = existingPriceDetails.EnvirontmentTax;
                        }
                    }
                }

                var priceDetailsToRemove = existingProduct.PriceDetails
                     .Where(existingPD => !updatedPriceDetails
                         .Any(updatedPD => updatedPD.PriceDetailID == existingPD.PriceDetailID))
                     .ToList();
                if (priceDetailsToRemove.Count > 0)
                {
                    piacomDbContext.PriceDetails.RemoveRange(priceDetailsToRemove);
                }

                await piacomDbContext.SaveChangesAsync();
                return existingProduct;
            }
            return null;
        }
    }
}
