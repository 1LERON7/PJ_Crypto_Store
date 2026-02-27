using System.ComponentModel.DataAnnotations;

namespace Crypto_Store.DTOs
{
    public class LoginUserDto
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }   
    }
}
