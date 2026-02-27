using Crypto_Store.DTOs;
using Crypto_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
                Title = p.Title,
                Description = p.Description,
                Price = p.PriceEth,
                ImageURL = p.ImageUrl
            }).ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ProductGetId(Guid id)
        {
            var product = await _db.Products.Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.PriceEth,
                    ImageURL = p.ImageUrl
                }).FirstOrDefaultAsync();

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> ProductCreate(ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool title = await _db.Products.AnyAsync(p => p.Title == dto.Title);

            if (title)
                return Conflict("A product with this name already exists.");

            var product = new Product
            {
                Title = dto.Title,
                Description = dto.Description,
                PriceEth = dto.Price,
                ImageUrl = dto.ImageURL
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return Ok(product);
        }
    }
}
