
using EmployeeApi.Models;

namespace Employees.Interfaces
{
    public interface IEmployee_Dept_loc
    {
        public Task<IEnumerable<Employee_Dept_Loc>> GetEmp_Dept_loc();

    }
}
