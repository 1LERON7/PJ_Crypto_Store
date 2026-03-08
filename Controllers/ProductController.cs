using Crypto_Store.DTOs;
using Crypto_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Crypto_Store.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
            int page = 1,
            int pageSize = 12,
            decimal? minPrice = null,
            string? search = null)
        {
            var query = _db.Products.AsQueryable();

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Title.Contains(search));


            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                items

            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ProductGetId(Guid id)
        {
            var product = await _db.Products.Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).FirstOrDefaultAsync();

            return Ok(product);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ProductCreate([FromBody] ProductDto dto)
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
                Price = dto.Price,
                ImageUrl = dto.ImageUrl
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return Ok(product);
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrice(Guid id, ProductDto dto)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product is not found");

            product.Title = dto.Title;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;


            await _db.SaveChangesAsync();

            return Ok(product);

        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p=> p.Id == id);
            if(product == null)
                return NotFound("Product is not found");

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
