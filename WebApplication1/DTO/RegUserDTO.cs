using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class RegUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PassWord { get; set; }
        [Compare("PassWord")]
        [Required]
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
