using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PiacomDbContext piacomDbContext;

        public EmployeeRepository(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;
        }
        public async Task<Employee> AddAsync(Employee employee)
        {
            await piacomDbContext.Employees.AddAsync(employee);
            await piacomDbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> DeleteAsync(Guid id)
        {
            var existingEmployee = await piacomDbContext.Employees.FindAsync(id);

            if (existingEmployee != null)
            {
                piacomDbContext.Employees.Remove(existingEmployee);
                await piacomDbContext.SaveChangesAsync();
                return existingEmployee;
            }
            return null;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await piacomDbContext.Employees.ToListAsync();
        }

        public Task<Employee?> GetAsync(Guid id)
        {
            return piacomDbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == id);
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {

            var existingEmployee = await piacomDbContext.Employees.FindAsync(employee.EmployeeID);
            if (existingEmployee != null)
            {
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Email = employee.Email;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.JobTitle = employee.JobTitle;
                await piacomDbContext.SaveChangesAsync();
                return existingEmployee;
            }
            return null;
        }
    }
}
