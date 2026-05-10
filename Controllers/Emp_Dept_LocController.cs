
using Employee_Dept_Loc_Proj.Services;
using Microsoft.AspNetCore.Mvc;


namespace Employee_Dept_Loc_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Emp_Dept_LocController : ControllerBase
    {
        private readonly DepartmentApiClient _departmentApiClient;

        public Emp_Dept_LocController(DepartmentApiClient departmentApiClient)
        {
            _departmentApiClient = departmentApiClient;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeewithDepart(int id,string JWTToken)
        {
            var department = await _departmentApiClient.GetDepartmentByIdAsync(id, JWTToken);
            return Ok(department);
        }
    }
}
