
using Employees.Models;

namespace Employees.Interfaces
{
    public interface IEmployee_Dept_loc
    {
        public Task<IEnumerable<Employees.Models.Employee_Dept_Loc>> GetEmp_Dept_loc();

    }
}
