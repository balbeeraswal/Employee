using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Employees.Interfaces;
using Employees.DbContxt;
using Employees.Models;
namespace Employees.Repositories
{
    public class EmployeeRepo:IEmployee
    {
        private  readonly DatabaseContext _dbcontext;
        public EmployeeRepo(DatabaseContext databaseContext ) {
            _dbcontext = databaseContext;
        }

        public async Task<IEnumerable<Employees.Models.Employee>> GetEmployees()
        {
            return await _dbcontext.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int EmployeeId)
        {
            return await _dbcontext.Employees.FirstOrDefaultAsync(e => e.EmpId == EmployeeId);

        }
        public async Task<Employee> AddEmployee(Employee employee)
        {
             _dbcontext.Employees.Add(employee);
            await _dbcontext.SaveChangesAsync();
            return employee;
        }

       
    }
}
