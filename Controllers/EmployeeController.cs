
using Microsoft.AspNetCore.Mvc;
using Employees.Interfaces;
using Employees.Models;


namespace Employees.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployee _employeeRepo;
        public EmployeeController(IEmployee employee, ILogger<EmployeeController> logger)
        {
            _employeeRepo = employee; _logger = logger;
        }

        [HttpGet("GetEmployees")]
        public async Task<ActionResult<IEnumerable<Employees.Models.Employee>>> GetEmployees()
        {
            var emp = await _employeeRepo.GetEmployees();
            if (emp == null)
            {
                return NotFound();
            }
            return Ok(emp);
        }

        [HttpGet("GetEmployeeById/{id}")]
        public async Task<ActionResult<ApiResponse<Employees.Models.Employee> >> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid Employee Id",
                    Data = null
                });
            }

           
                var result = await _employeeRepo.GetEmployeeById(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Employee not found",
                        Data = null
                    });
                }

            return Ok(result);
               
           
        }


        [HttpPost("AddEmployee")]
        public async Task<ActionResult<ApiResponse<Employees.Models.Employee>>> AddEmployee([FromBody] Employees.Models.Employee employee)
        {
            if (_employeeRepo == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid Employee Details Given",
                    Data = null
                });
            }
            try
            {
                var result = await _employeeRepo.AddEmployee(employee);
                //return Ok(new ApiResponse<object>
                //{
                //    StatusCode = StatusCodes.Status200OK,
                //    Message = "Employee has been Added Successfully",
                //    Data = result
                //});

                return CreatedAtAction(nameof(GetEmployeeById),new { id = result.EmpId }, new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Employee has been Added Successfully",
                    Data = result
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There is some error in adding employee");

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
                {
                    StatusCode=StatusCodes.Status500InternalServerError,
                    Message= ex.Message,
                    Data=null

                });

            }

        }

        [HttpGet("TestException")]
        public IActionResult TestException()
        {
            throw new InvalidOperationException("This is a test exception");
        }
    }
}

