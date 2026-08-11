using System.ComponentModel.DataAnnotations;

namespace Crypto_Store.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string Username { get; set; }
    }
}
