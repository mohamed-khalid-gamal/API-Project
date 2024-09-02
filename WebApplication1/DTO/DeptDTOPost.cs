using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class DeptDTOPost
    {
        
      
        [Required]
        public string Name { get; set; }
        [Required]
        public string Manager { get; set; }
    }
}
