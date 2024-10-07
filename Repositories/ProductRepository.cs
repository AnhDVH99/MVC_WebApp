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
            return piacomDbContext.Products.FirstOrDefaultAsync(x => x.ProductID == id);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProduct = await piacomDbContext.Products.FindAsync(product.ProductID);
            if (existingProduct != null)
            {
                existingProduct.ProductCode = product.ProductCode;
                existingProduct.ProductName = product.ProductName;
                existingProduct.ProductStatus = product.ProductStatus;
                existingProduct.ProductDescription = product.ProductDescription;
                await piacomDbContext.SaveChangesAsync();
                return existingProduct;
            }
            return null;
        }
    }
}
