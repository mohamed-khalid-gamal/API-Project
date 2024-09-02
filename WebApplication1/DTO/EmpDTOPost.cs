using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class EmpDTOPost
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string grade { get; set; }
        [Required]
        public int DepId { get; set; }
    }
}
