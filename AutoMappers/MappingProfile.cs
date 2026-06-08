using AutoMapper;
using EmployeeApi.DTOs;
using EmployeeApi.Models;

namespace Employees.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. Map Employee <-> EmployeeDto (Handles both directions)
            CreateMap<Employee, EmployeeDto>().ReverseMap();

            // 2. Map CreateEmployeeDto -> Employee (Used for POST/Creation requests)
            CreateMap<CreateEmployeeDto, Employee>();

            // 3. Map Employee_Dept_Loc -> EmployeeDeptLocDto
            // Since the property names match perfectly (EmpId, EmpName, DeptName, LocName),
            // AutoMapper pairs them up automatically!
            CreateMap<Employee_Dept_Loc, EmployeeDeptLocDto>();
        }
    }
}