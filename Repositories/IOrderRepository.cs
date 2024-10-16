﻿using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();

        Task<Order?> GetAsync(Guid id);

        Task<Order> AddAsync(Order order);

        Task<Order?> UpdateAsync(Order order);

        Task<Order?> DeleteAsync(Guid id);

        Task<(float VAT, float EnvironmentTax, float Price, float PriceBeforeTax)?> GetProductTaxesAsync(Guid productId);

        Task<PriceDetail?> GetProductPriceDetailByOrderAsync(Guid productID, DateTime orderDate);

        Task<IEnumerable<Order>> GetOrderByCustomerAndDateRange(Guid cusId, DateTime orderDate);

        Task<IEnumerable<Order>> GetAllOrder();

        Task<IEnumerable<Order>> SearchOrdersAsync(string searchTerm);
    }
}
