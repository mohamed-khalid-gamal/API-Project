using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class LogDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
