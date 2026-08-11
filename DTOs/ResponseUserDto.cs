using System.ComponentModel.DataAnnotations;

namespace Crypto_Store.DTOs
{
    public class ResponseUserDto
    {
        public Guid Id { get; set; }

        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }

    }
}
