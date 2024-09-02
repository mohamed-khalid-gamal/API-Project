using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class EmployeeWithDepartmentDataDTO
    {
        
        public int Id { get; set; }
      
        public string EmployeeName { get; set; }
      
        public string grade { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentManager { get; set; }
    }
}
