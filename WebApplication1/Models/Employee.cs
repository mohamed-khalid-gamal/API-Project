using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string grade { get; set; }
        [ForeignKey(name: "Department")]
        public int DepId { get; set; }
        public virtual Department Department { get; set; }
    }
}
