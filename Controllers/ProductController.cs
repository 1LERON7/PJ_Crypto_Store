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
        public async Task<IActionResult> GetProducts(
            int page = 1,
            int pageSize = 10,
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
            var product = await _db.Products.Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
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

            var product = new product
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageURL
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return Ok(product);
        }


        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/title")]
        public async Task<IActionResult> UpdateTitle(Guid id, [FromBody] string title)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound("Product is not found");

            product.Title = title;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/descripton")]
        public async Task<IActionResult> UpdateDescrition(Guid id, [FromBody] string description)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound("Product is not found");

            product.Description = description;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/price")]
        public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] decimal price)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p=> p.Id == id);
            if(product == null)
                return NotFound("Product is not found");

            product.Price = price;
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/image")]
        public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] string imageUrl)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound("Product is not found");

            product.ImageUrl = imageUrl;
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/delete")]
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
