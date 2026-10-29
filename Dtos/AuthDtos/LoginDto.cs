using System.ComponentModel.DataAnnotations;

namespace Identity_template.Dtos.AuthDtos
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
