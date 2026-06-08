
using EmployeeApi.DTOs;
using EmployeeApi.Models;

namespace Employees.Interfaces
{
    public interface IEmployee
    {
        public  Task<IEnumerable<Employee>> GetEmployees();
        public Task<Employee> GetEmployeeById(int id);
        Task<Employee> AddEmployee(Employee employee);
        Task<bool> DeleteEmployee(int id);
        Task<Employee> UpdateEmployee(Employee employee);
        Task<IEnumerable<EmployeeDeptLocDto>> GetEmployeeDeptLocDetails();
        Task<IEnumerable<Employee>> FetchEmployeesWithKeySetPagination(int? lastEmpId = null);

    }


}
