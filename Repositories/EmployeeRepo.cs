using System.Collections.Immutable;
using System.Linq;
using EmployeeApi.DTOs;
using EmployeeApi.Models;
using Employees.DbContxt;
using Employees.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Employees.Repositories
{
    public class EmployeeRepo : IEmployee
    {
        private readonly DatabaseContext _dbcontext;
        private readonly ILogger<EmployeeRepo> _logger;
        public EmployeeRepo(DatabaseContext databaseContext, ILogger<EmployeeRepo> logger)
        {

            _dbcontext = databaseContext; _logger = logger;

        }

        public async Task<IEnumerable<Employee>?> GetEmployees()
        {
            _logger.LogInformation("Fetching All Employee at {Time}", DateTime.UtcNow);
            var result= await _dbcontext.Employees.ToListAsync();
            if (result.Any()) {
                _logger.LogWarning("Employee List Retrived at {Time}", DateTime.UtcNow);
                return result;
            }
            else
            {
                _logger.LogWarning("No Employee Found at {Time}", DateTime.UtcNow);
                return null;
            }

        }

        public async Task<Employee?> GetEmployeeById(int EmployeeId)
        {
            _logger.LogInformation("Fetching Employee with {Id}", EmployeeId);
             var result=await _dbcontext.Employees.FirstOrDefaultAsync(e => e.EmpId == EmployeeId);
            if (result != null) {
                _logger.LogWarning("Employee with Id {Id} Fetched at {Time}", EmployeeId, DateTime.UtcNow);
                return result;
            }
            else
            {
                _logger.LogWarning("Employee with Id {Id} not found at {Time}", EmployeeId, DateTime.UtcNow);
                return null;
            }

        }
        public async Task<Employee> AddEmployee(Employee employee)
        {
            _logger.LogInformation("Adding Employee with Id {Id} with the Department {deptid} at {Time}", employee.EmpId, employee.DeptId, DateTime.UtcNow);
            _dbcontext.Employees.Add(employee);
            var result = await _dbcontext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation("Employee with Emp Id:-{EmpId},Department Id:-{DeptId}, Added Successfull at {Time}", employee.EmpId, employee.DeptId, DateTime.UtcNow);
                return employee;
            }
            else
            {
                _logger.LogInformation("Employee with Emp Id:-{EmpId},Department Id:-{DeptId}, failed to be added at {Time}", employee.EmpId, employee.DeptId, DateTime.UtcNow);
                return null;
            }

        }

        public async Task<bool> DeleteEmployee(int id)
        {

            if (id == 0) return false;
            var emp = await _dbcontext.Employees.FindAsync(id);
            if (emp == null) return false;
            _dbcontext.Employees.Remove(emp);
            var result = await _dbcontext.SaveChangesAsync();
            if (result > 0)
            {
                _logger.LogInformation("Adding Employee with Id {Id} has been deleted Successfully at {Time}", id, DateTime.UtcNow);
                return true;
            }
            else
            {
                _logger.LogError("Failed To Delete Employee With Id {id} at time{Time}", id, DateTime.UtcNow);
                return false;
            }

        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var existing = await _dbcontext.Employees.FirstAsync(id => id.EmpId == employee.EmpId);
            if (existing == null)
            {
                return null;
            }

            existing.LocId = employee.LocId;
            existing.DeptId = employee.DeptId;
            

            var result = await _dbcontext.SaveChangesAsync();
            if (result > 0)
            {
                _logger.LogInformation("Employee with Id {Id} has been Updated Successfully at {Time}", employee.EmpId, DateTime.UtcNow);
                return existing;
            }
            else
            {
                _logger.LogError("Failed To Update Employee With Id {id} at time{Time}", employee.EmpId, DateTime.UtcNow);
                return null;
            }

        }
     
        public async Task<IEnumerable<Employee>?> FetchEmployeesWithKeySetPagination(int? lastEmpId = null)
        {
            _logger.LogInformation("Fetching Employees at {Time}", DateTime.UtcNow);

            int pageSize = 5;

            IQueryable<Employee> query = _dbcontext.Employees.OrderBy(e => e.EmpId);

            // Apply keyset condition if lastEmpId is provided
            if (lastEmpId.HasValue)
            {
                query = query.Where(e => e.EmpId > lastEmpId.Value);
            }

            var result = await query.Take(pageSize).ToListAsync();

            if (result.Any())
            {
                _logger.LogInformation("Employee page retrieved at {Time}", DateTime.UtcNow);
                return result;
            }
            else
            {
                _logger.LogWarning("No Employees found at {Time}", DateTime.UtcNow);
                return null;
            }

        }






        public async Task<IEnumerable<EmployeeDeptLocDto>> GetEmployeeDeptLocDetails()
        {
            var result = await _dbcontext.Employees.AsNoTracking().Include(d => d.Department).Include(l => l.Location).ToListAsync();
            if(!result.Any())
            {
                _logger.LogWarning("No Employee Found at {Time}", DateTime.UtcNow);
                return Enumerable.Empty<EmployeeDeptLocDto>();
            }
           
                _logger.LogInformation("Employee List with Department and Location Details Retrieved at {Time}", DateTime.UtcNow);

            var empDeptLocDetails = result.Select(edl => new EmployeeDeptLocDto()
            {
                EmpId = edl.EmpId,
                EmpName = edl.EmpName,
                DeptName = edl.Department.DeptName,
                LocName = edl.Location.LocName
            });

            return empDeptLocDetails;
            
        }

      
    }
}
