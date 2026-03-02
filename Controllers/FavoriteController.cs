using Crypto_Store.DTOs;
using Crypto_Store.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Crypto_Store.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FavoriteController(AppDbContext db) => _db = db;


        [HttpGet]
        public async Task<IActionResult> GetMyFavorites()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null) return 
                    Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var ids = await _db.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.ProductId)
                .ToListAsync();

            return Ok(ids);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] FavoriteDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null) 
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var exists = await _db.Favorites.AnyAsync(f => f.UserId == userId && f.ProductId == dto.ProductId);
            if (exists) 
                return Ok(new { added = false });

            _db.Favorites.Add(new Favorite { UserId = userId, ProductId = dto.ProductId });
            await _db.SaveChangesAsync();

            return Ok(new { added = true });
        }




        [HttpDelete("remove/{productId:guid}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null) 
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var fav = await _db.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);


            if (fav == null) 
                return Ok(new { removed = false });

            _db.Favorites.Remove(fav);
            await _db.SaveChangesAsync();

            return Ok(new { removed = true });
        }
    }
}
