using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;

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
            return piacomDbContext.Orders.FirstOrDefaultAsync(x => x.OrderID == id);

        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            var existingOrder = await piacomDbContext.Orders.FindAsync(order.OrderID);
            if (existingOrder != null)
            {
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.RequiredDate = order.RequiredDate;
                existingOrder.ShippedDate = order.ShippedDate;
                existingOrder.OrderStatus = order.OrderStatus;
                existingOrder.Comment = order.Comment;
                existingOrder.CustomerID = order.CustomerID;
                existingOrder.EmployeeID = order.EmployeeID;
                existingOrder.SysU = order.SysU;
                existingOrder.SysD = order.SysD;

                await piacomDbContext.SaveChangesAsync();
                return existingOrder;
            }
            return null;
        }
    }
}
