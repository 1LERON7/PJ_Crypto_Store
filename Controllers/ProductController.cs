using Crypto_Store.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crypto_Store.DTOs;

namespace Crypto_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> ProductsGetAll()
        {
            var products = await _db.Products.Select(p => new ProductDto
            {
                title = p.Title,
                description = p.Description,
                price = p.PriceEth,
                image_URL = p.ImageUrl
            }).ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ProductGetId(Guid id)
        {
            var product = await _db.Products.Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    title = p.Title,
                    description = p.Description,
                    price = p.PriceEth,
                    image_URL = p.ImageUrl
                }).FirstOrDefaultAsync();

            return Ok(product);
        }

        //[HttpPost("create")]
        //public async Task<IActionResult> ProductCreate()
        //{

        //}
    }
}
