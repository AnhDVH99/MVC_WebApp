using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using OfficeOpenXml;

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

        public async Task<byte[]> ExportCustomersToExcelAsync()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var customers = await piacomDbContext.Customers.ToListAsync();
            var employees = await piacomDbContext.Employees.ToListAsync();


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Customers");
                worksheet.Cells["A1"].Value = "Customer ID";
                worksheet.Cells["B1"].Value = "Customer Name";
                worksheet.Cells["C1"].Value = "Phone";
                worksheet.Cells["D1"].Value = "Address";
                worksheet.Cells["E1"].Value = "City";
                worksheet.Cells["F1"].Value = "State";
                worksheet.Cells["G1"].Value = "Postal Code";
                worksheet.Cells["H1"].Value = "Country";
                worksheet.Cells["I1"].Value = "SaleRepID";
                worksheet.Cells["J1"].Value = "Sale Representative";

                int row = 2;
                foreach (var customer in customers)
                {
                    worksheet.Cells[row, 1].Value = customer.CustomerID;
                    worksheet.Cells[row, 2].Value = customer.CustomerName;
                    worksheet.Cells[row, 3].Value = customer.Phone;
                    worksheet.Cells[row, 4].Value = customer.AddressLine1 + " " + customer.AddressLine2;
                    worksheet.Cells[row, 5].Value = customer.City;
                    worksheet.Cells[row, 6].Value = customer.State;
                    worksheet.Cells[row, 7].Value = customer.PostalCode;
                    worksheet.Cells[row, 8].Value = customer.Country;
                    worksheet.Cells[row, 9].Value = customer.SaleRepEmployeeID;
                    var saleRepEmployee = employees.FirstOrDefault(e => e.EmployeeID == customer.SaleRepEmployeeID);
                    worksheet.Cells[row, 10].Value = saleRepEmployee != null
                        ? $"{saleRepEmployee.FirstName} {saleRepEmployee.LastName}"
                        : "N/A"; // Default value if no employee is assigned

                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                return package.GetAsByteArray();
            }

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

        public async Task<CreditLimit?> GetCreditLimitByCustomerAndDateAsync(Guid cusId, DateTime orderDate)
        {
            return await piacomDbContext.CreditLimits
                .Where(c => c.CustomerID == cusId && c.FromDate <= orderDate && c.ToDate >= orderDate)
                .OrderByDescending(c => c.FromDate)
                .FirstOrDefaultAsync();
        }

        public async Task ImportCustomersFromExcelAsync(Stream fileStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Assuming first worksheet
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var customerName = worksheet.Cells[row, 2].Value?.ToString();
                    var phone = worksheet.Cells[row, 3].Value?.ToString();
                    var address = worksheet.Cells[row, 4].Value?.ToString();
                    var city = worksheet.Cells[row, 5].Value?.ToString();
                    var state = worksheet.Cells[row, 6].Value?.ToString();
                    var postalCode = worksheet.Cells[row, 7].Value?.ToString();
                    var country = worksheet.Cells[row, 8].Value?.ToString();
                    var saleRepEmployeeID = Guid.Parse(worksheet.Cells[row, 9].Value?.ToString());

                    var customer = new Customer
                    {
                        CustomerName = customerName,
                        Phone = phone,
                        AddressLine1 = address,  // If address is split across columns, adjust here
                        City = city,
                        State = state,
                        PostalCode = postalCode,
                        Country = country,
                        SaleRepEmployeeID = saleRepEmployeeID
                    };

                    piacomDbContext.Customers.Add(customer); // Add customer to the database
                }

                await piacomDbContext.SaveChangesAsync();
            }
        }

        public bool isWithinCreditLimit(CreditLimit creditLimit, decimal orderTotalAmount)
        {
            if (creditLimit == null) return false;
                return orderTotalAmount <= creditLimit.Total;
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
