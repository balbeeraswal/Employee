using System.Text.Json.Serialization;

namespace EmployeeApi.DTOs
{
    public class EmployeeDto
    {
        [JsonIgnore]
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int DeptId { get; set; }
        public int LocId { get; set; }
    }

    public class CreateEmployeeDto
    {
        public string EmpName { get; set; }
        public int DeptId { get; set; }
        public int LocId { get; set; }
    }

    public class EmployeeDeptLocDto
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string DeptName { get; set; }
        public string LocName { get; set; }
    }
}
