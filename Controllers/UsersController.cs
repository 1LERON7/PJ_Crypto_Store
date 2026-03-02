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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        [Authorize] // проверка запроса на авторизацию. (валидный JWT токен).
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            // Id юзера из JWT Token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

            if (user == null)
                return NotFound();

            return Ok(new
            {
                email = user.Email,
                role = user.Role,
                createdAt = user.Created
            });
        }


        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _db.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
                return Unauthorized();

            // проверка старого пароля (хэш)
            var isValid = BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash);
            if (isValid == false)
                return BadRequest("Old password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            user.RefreshToken = null;
            user.RefreshTokenTime = null;

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
