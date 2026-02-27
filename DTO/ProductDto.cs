using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;

namespace Crypto_Store.DTO
{
    //    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),       // не для пользователя
    //title VARCHAR(50) NOT NULL,       // для пользователя
    //description TEXT,                 // для пользователя
    //price_ETH INT NOT NULL,           // для пользователя
    //image_URL TEXT NOT NULL,          // для пользователя

    //created TIMESTAMP DEFAULT NOW()       // не для пользователя
    public class ProductDto
    {
        public string title {  get; set; }
        public string description { get; set; }
        public float price {  get; set; }
        public string image_URL {  get; set; }
    }
}
