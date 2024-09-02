using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        //[JsonIgnore] // solution 1 for cycle exception (not best solution )
        //solution 2 is DTO (data transefer object) 
        public virtual List<Employee> Employees { get; set; }
    }
}
