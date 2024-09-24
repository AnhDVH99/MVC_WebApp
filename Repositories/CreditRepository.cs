    using ASP.NET_Core_MVC_Piacom.Data;
    using ASP.NET_Core_MVC_Piacom.Models.Domain;
    using Microsoft.EntityFrameworkCore;

    namespace ASP.NET_Core_MVC_Piacom.Repositories
    {
        public class CreditRepository : ICreditRepository
        {
            private readonly PiacomDbContext piacomDbContext;

            public CreditRepository(PiacomDbContext piacomDbContext) 
            {
                this.piacomDbContext = piacomDbContext;
            }
            public async Task<CreditLimit> AddAsync(CreditLimit creditLimit)
            {
                await piacomDbContext.CreditLimits.AddAsync(creditLimit);
                await piacomDbContext.SaveChangesAsync();
                return creditLimit;
            }

            public async Task<CreditLimit?> DeleteAsync(Guid id)
            {
                var existingCredit = await piacomDbContext.CreditLimits.FindAsync(id);

                if (existingCredit != null)
                {
                    piacomDbContext.CreditLimits.Remove(existingCredit);
                    await piacomDbContext.SaveChangesAsync();
                    return existingCredit;
                }
                return null;
            }

            public async Task<IEnumerable<CreditLimit>> GetAllAsync()
            {
                return await piacomDbContext.CreditLimits
                   .Include(cl => cl.Customer)
                   .ToListAsync();
            }

            public async Task<CreditLimit?> GetAsync(Guid id)
            {
                return await piacomDbContext.CreditLimits.FirstOrDefaultAsync(x => x.CreditLimitID == id);

            }


            public async Task<CreditLimit?> UpdateAsync(CreditLimit creditLimit)
            {
                var existingCredit = await piacomDbContext.CreditLimits.FindAsync(creditLimit.CreditLimitID);
                if (existingCredit != null)
                {
                    existingCredit.CreditLimitID = creditLimit.CreditLimitID;
                    existingCredit.CustomerID = creditLimit.CustomerID;
                    existingCredit.FromDate = creditLimit.FromDate;
                    existingCredit.ToDate = creditLimit.ToDate;
                    existingCredit.CreditType = creditLimit.CreditType;
                    existingCredit.Total = creditLimit.Total;
                    existingCredit.OverDue = creditLimit.OverDue;

                    await piacomDbContext.SaveChangesAsync();
                    return existingCredit;
                }
                return null;
            }
        }
    }
