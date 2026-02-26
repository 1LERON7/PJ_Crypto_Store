using Crypto_Store.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crypto_Store.DTO;

namespace Crypto_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ProductsGetAll()
        {
            var products = await _context.Products.Select(p => new ProductDto
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
            var product = await _context.Products.Where(p => p.Id == id)
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
