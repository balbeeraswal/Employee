using System.Collections.Immutable;
using Employees.DbContxt;
using Employees.Interfaces;
using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Employees.Repositories
{
    public class Employee_Dept_Loc_Repo: IEmployee_Dept_loc
    {
        private  readonly DatabaseContext _dbcontext;
        public Employee_Dept_Loc_Repo(DatabaseContext databaseContext ) {
            _dbcontext = databaseContext;
        }

    
        Task<IEnumerable<Employee_Dept_Loc>> IEmployee_Dept_loc.GetEmp_Dept_loc()
        {
            throw new NotImplementedException();
        }
    }
}
