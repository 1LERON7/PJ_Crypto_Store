using System.ComponentModel.DataAnnotations.Schema;

namespace Crypto_Store.DTOs
{
    public class CreateUserDto
    {
        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
