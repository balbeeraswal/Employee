using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Employees.Models
{
    public class Employee
    {
        [Key]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }

        public string EmpName { get; set; }
        public int DeptId { get; set; }
        public int LocId { get; set; }

    }

    public class Employee_Dept_Loc
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int LocId { get; set; }

        public string LocName { get; set; }

    }



    //public class Department
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [JsonIgnore]
    //    public int DeptId { get; set; }

    //    public string DeptName { get; set; }

    //}

    //public class Location
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    [JsonIgnore]

    //    public int LocId { get; set; }

    //    public string LocName { get; set; }

    //}
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int DeptId { get; set; }

        public string DeptName { get; set; }

    }
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
