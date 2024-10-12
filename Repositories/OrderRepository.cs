using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PiacomDbContext piacomDbContext;

        public OrderRepository(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;
        }
        public async Task<Order> AddAsync(Order order)
        {
            await piacomDbContext.Orders.AddAsync(order);
            await piacomDbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> DeleteAsync(Guid id)
        {
            var existingOrder = await piacomDbContext.Orders.FindAsync(id);

            if (existingOrder != null)
            {
                piacomDbContext.Orders.Remove(existingOrder);
                await piacomDbContext.SaveChangesAsync();
                return existingOrder;
            }
            return null;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await piacomDbContext.Orders
                .Include(o => o.Customer)
                .ToListAsync();
        }

        public Task<Order?> GetAsync(Guid id)
        {
            return piacomDbContext.Orders.Include(od => od.OrderDetails).FirstOrDefaultAsync(x => x.OrderID == id);

        }
        public async Task<(float VAT, float EnvironmentTax, float Price, float PriceBeforeTax)?> GetProductTaxesAsync(Guid productId)
        {
            var priceDetail = await piacomDbContext.PriceDetails
                .Include(pd => pd.PriceNav)
                .FirstOrDefaultAsync(pd => pd.ProductID == productId);

            if (priceDetail == null)
            {
                return null;
            }

            return (
                priceDetail.VAT,
                priceDetail.EnvirontmentTax,
                priceDetail.Price,
                priceDetail.PriceBeforeTax
                
                );
        }

        public async Task<PriceDetail?> GetProductPriceDetailByOrderAsync(Guid productId, DateTime orderDate)
        {

            // Retrieve the PriceDetail based on product, order date, and Price model's FromDate and ToDate
            var priceDetail = await piacomDbContext.PriceDetails
                .Include(pd => pd.PriceNav) // Include Price navigation property
                .Where(pd => pd.ProductID == productId
                    && pd.PriceNav.FromDate <= orderDate
                    && pd.PriceNav.ToDate >= orderDate)  // Ensure that the order date falls between the price's date range
                .OrderByDescending(pd => pd.PriceNav.FromDate)
                .FirstOrDefaultAsync();

            return priceDetail;
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            var existingOrder = await piacomDbContext.Orders
                .Include(c => c.OrderDetails)
                .FirstOrDefaultAsync(c => c.OrderID == order.OrderID);
            if (existingOrder != null)
            {
                //Update customer entity
                piacomDbContext.Entry(existingOrder).CurrentValues.SetValues(order);

                var updatedOrderDetails = order.OrderDetails ?? new List<OrderDetail>();

                var newOrderDetails = updatedOrderDetails
                    .Where(updatePD => updatePD.OrderDetailID == Guid.Empty)
                    .ToList();

                var existingOrderDetailsToUpdate = updatedOrderDetails
                    .Where(updateOD => existingOrder.OrderDetails
                    .Any(existingOD => existingOD.OrderDetailID == updateOD.OrderDetailID))
                    .ToList();


                var lstNotExistOrderDetail = new List<OrderDetail>();

                if (lstNotExistOrderDetail.Count > 0)
                    existingOrder.OrderDetails.AddRange(lstNotExistOrderDetail);


                if (newOrderDetails.Count > 0)
                {
                    foreach (var newOrderDetail in newOrderDetails)
                    {
                        newOrderDetail.OrderID = order.OrderID;
                        var priceDetail = await piacomDbContext.PriceDetails
                            .FirstOrDefaultAsync(pd => pd.ProductID == newOrderDetail.ProductID);
                        if (priceDetail != null)
                        {
                            newOrderDetail.VAT = priceDetail.VAT;
                            newOrderDetail.EnvironmentTax = priceDetail.EnvirontmentTax;
                        }
                    }
                }
                existingOrder.OrderDetails.AddRange(newOrderDetails);


                if (existingOrderDetailsToUpdate.Count > 0)
                {
                    foreach (var existingOrderDetails in existingOrderDetailsToUpdate)
                    {
                        var dbOrderDetail = existingOrder.OrderDetails
                            .FirstOrDefault(od => od.OrderDetailID == existingOrderDetails.OrderDetailID);

                        if (dbOrderDetail != null)
                        {
                            dbOrderDetail.UnitID = existingOrderDetails.UnitID;
                            dbOrderDetail.ProductID = existingOrderDetails.ProductID;
                            dbOrderDetail.Quantity = existingOrderDetails.Quantity;
                            dbOrderDetail.Price = existingOrderDetails.Price;
                            dbOrderDetail.TotalAmount = existingOrderDetails.TotalAmount;
                            dbOrderDetail.priceAfterTax = existingOrderDetails.priceAfterTax;
                            dbOrderDetail.priceBeforeTax = existingOrderDetails.priceBeforeTax;
                            dbOrderDetail.VAT = existingOrderDetails.VAT;
                            dbOrderDetail.EnvironmentTax = existingOrderDetails.EnvironmentTax;
                        }
                    }
                }

                var orderDetailsToRemove = existingOrder.OrderDetails
                     .Where(existingOD => !updatedOrderDetails
                         .Any(updatedOD => updatedOD.OrderDetailID == existingOD.OrderDetailID))
                     .ToList();
                if (orderDetailsToRemove.Count > 0)
                {
                    piacomDbContext.OrderDetails.RemoveRange(orderDetailsToRemove);
                }

                await piacomDbContext.SaveChangesAsync();
                return existingOrder;
            }
            return null;
        }
    }
}
