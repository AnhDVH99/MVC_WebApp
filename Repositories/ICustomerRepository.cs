﻿using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();

        Task<Customer?> GetAsync(Guid id);

        Task<Customer> AddAsync(Customer customer);

        Task<Customer?> UpdateAsync(Customer customer);

        Task<Customer?> DeleteAsync(Guid id);

        Task<Customer?> GetByIdAsync(Guid id);
    }
}