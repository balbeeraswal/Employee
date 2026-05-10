
using Employees.Models;

namespace Employees.Interfaces
{
    public interface IEmployee
    {
        public  Task<IEnumerable<Employee>> GetEmployees();
        public Task<Employee> GetEmployeeById(int id);
        public  Task<Employee> AddEmployee(Employees.Models.Employee employee);
    }

   
}
