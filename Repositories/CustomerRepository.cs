using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PiacomDbContext piacomDbContext;

        public CustomerRepository(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            await piacomDbContext.Customers.AddAsync(customer);
            await piacomDbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> DeleteAsync(Guid id)
        {
            var existingCustomer = await piacomDbContext.Customers.FindAsync(id);

            if (existingCustomer != null)
            {
                piacomDbContext.Customers.Remove(existingCustomer);
                await piacomDbContext.SaveChangesAsync();
                return existingCustomer;
            }
            return null;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await piacomDbContext.Customers.ToListAsync();
        }

        public Task<Customer?> GetAsync(Guid id)
        {
            return piacomDbContext.Customers.Include(c => c.CreditLimits).FirstOrDefaultAsync(x => x.CustomerID == id);
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await piacomDbContext.Customers
            .Include(c => c.CreditLimits) // Include related CreditLimits
            .FirstOrDefaultAsync(c => c.CustomerID == id); // Query by CustomerID
        }

        public async Task<Customer?> UpdateAsync(Customer customer)
        {
            var existingCustomer = await piacomDbContext.Customers
                .Include(c => c.CreditLimits)
                .FirstOrDefaultAsync(c => c.CustomerID == customer.CustomerID);
            if (existingCustomer != null)
            {
                //Update customer entity
                piacomDbContext.Entry(existingCustomer).CurrentValues.SetValues(customer);

                var updatedCreditLimits = customer.CreditLimits ?? new List<CreditLimit>();

                var newCreditLimits = updatedCreditLimits
                    .Where(updateCL => updateCL.CreditLimitID == Guid.Empty)
                    .ToList();

                var existingCreditLimitsToUpdate = updatedCreditLimits
                    .Where(updateCL => existingCustomer.CreditLimits
                    .Any(existingCL => existingCL.CreditLimitID == updateCL.CreditLimitID))
                    .ToList();

                var lstNotExistCreditlimit = new List<CreditLimit>();

                if (lstNotExistCreditlimit.Count > 0)
                    existingCustomer.CreditLimits.AddRange(lstNotExistCreditlimit);

                if (newCreditLimits.Count > 0)
                {
                    foreach (var newLimit in newCreditLimits)
                    {
                        newLimit.CustomerID = customer.CustomerID;
                    }
                    existingCustomer.CreditLimits.AddRange(newCreditLimits);
                }

                if (existingCreditLimitsToUpdate.Count > 0)
                {
                    foreach (var existingLimit in existingCreditLimitsToUpdate)
                    {
                        var dbLimit = existingCustomer.CreditLimits
                            .FirstOrDefault(cl => cl.CreditLimitID == existingLimit.CreditLimitID);

                        if (dbLimit != null)
                        {
                            dbLimit.Total = existingLimit.Total;
                            dbLimit.FromDate = existingLimit.FromDate;
                            dbLimit.ToDate = existingLimit.ToDate;
                        }
                    }
                }

                var creditLimitsToRemove = existingCustomer.CreditLimits
              .Where(existingCL => !customer.CreditLimits
                  .Any(updatedCL => updatedCL.CreditLimitID == existingCL.CreditLimitID))
              .Take(updatedCreditLimits.Count)  // Limit by the length of updatedCreditLimits
              .ToList();
                if (creditLimitsToRemove.Count > 0)
                {
                    piacomDbContext.CreditLimits.RemoveRange(creditLimitsToRemove);
                }

                await piacomDbContext.SaveChangesAsync();
                return existingCustomer;
            }
            return null;
        }
    }
}
