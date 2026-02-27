using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Crypto_Store.DTOs
{
    //    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),       // не для пользователя
    //title VARCHAR(50) NOT NULL,       // для пользователя
    //description TEXT,                 // для пользователя
    //price_ETH INT NOT NULL,           // для пользователя
    //image_URL TEXT NOT NULL,          // для пользователя

    //created TIMESTAMP DEFAULT NOW()       // не для пользователя
    public class ProductDto
    {
        // [Required(ErrorMessage = "катомный месседж")]
        // атрибуты валидации. Супер темка для проверки!!!!
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        [Range(0.1, float.MaxValue)]
        public decimal Price {  get; set; }

        [Url]
        public string? ImageURL {  get; set; } = string.Empty;
    }
}
