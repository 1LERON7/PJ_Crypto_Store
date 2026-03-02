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
            var query = _db.products.AsQueryable();

            if (minPrice.HasValue)
                query = query.Where(p => p.price >= minPrice);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.title.Contains(search));


            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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
            var product = await _db.products.Where(p => p.id == id)
                .Select(p => new ProductDto
                {
                    Title = p.title,
                    Description = p.description,
                    Price = p.price,
                    ImageURL = p.image_url
                }).FirstOrDefaultAsync();

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> ProductCreate(ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool title = await _db.products.AnyAsync(p => p.title == dto.Title);

            if (title)
                return Conflict("A product with this name already exists.");

            var product = new product
            {
                title = dto.Title,
                description = dto.Description,
                price = dto.Price,
                image_url = dto.ImageURL
            };

            _db.products.Add(product);
            await _db.SaveChangesAsync();

            return Ok(product);
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/title")]
        public async Task<IActionResult> UpdateTitle(Guid id, [FromBody] string title)
        {
            var product = await _db.products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
                return NotFound("Product is not found");

            product.title = title;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/descripton")]
        public async Task<IActionResult> UpdateDescrition(Guid id, [FromBody] string description)
        {
            var product = await _db.products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
                return NotFound("Product is not found");

            product.description = description;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/price")]
        public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] decimal price)
        {
            var product = await _db.products.FirstOrDefaultAsync(p=> p.id == id);
            if(product == null)
                return NotFound("Product is not found");

            product.price = price;
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/image")]
        public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] string imageUrl)
        {
            var product = await _db.products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null)
                return NotFound("Product is not found");

            product.image_url = imageUrl;
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _db.products.FirstOrDefaultAsync(p=> p.id == id);
            if(product == null)
                return NotFound("Product is not found");

            _db.products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
