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

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return BadRequest();

            var user = await _db.Users.FindAsync(Guid.Parse(userId));

            user.Bio = dto.Bio;
            user.GamerTag = dto.Tag;

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("username")]
        public async Task<IActionResult> UpdateUsername(string name)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return BadRequest();

            var user = await _db.Users.FindAsync(Guid.Parse(userId));

            user.Username = name;

            await _db.SaveChangesAsync();

            return Ok();
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
                avatar = user.AvatarUrl,
                bio = user.Bio,
                tag = user.GamerTag,
                name = user.Username,
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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            int page = 1,
            int pageSize = 12,
            DateTime? createdAfter = null,
            string? search = null,
            string? sort = null)
        {
            
            var query = _db.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                query = query.Where(u => u.Email.Contains(search));
            }
                

            if (sort == "newest")
            {
                query = query.OrderByDescending(u => u.Created);
            }
            else if (sort == "oldest")
            {
                query = query.OrderBy(u => u.Created);
            }
            else
            {
                query = query.OrderByDescending(u => u.Created);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(u => u.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                items
            });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var user = new User
            {
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Created = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(); 
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound("User is not found");

            _db.Users.Remove(user);
            _db.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}/role")]
        public async Task<IActionResult> ChangeRoleUser(Guid id, string role)
        {
            var allowedRoles = new[] { "admin", "user" };

            if (!allowedRoles.Contains(role))
                return BadRequest("Invalid role");

            //var user = await _db.Users.FindAsync(id);
            //if (user == null)
            //    return NotFound("User is not found");

            //user.Role = role;

            //await _db.SaveChangesAsync();

            var rows = await _db.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.Role, role));

            if(rows == 0)
                return NotFound();

            return NoContent();
        }


        [Authorize]
        [HttpPost("wallet")]
        public async Task<IActionResult> SaveWallet([FromBody] WalletDto dto)
        {
            if (dto == null)
                return BadRequest("DTO is null");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return BadRequest();

            var user = await _db.Users.FindAsync(Guid.Parse(userId));

            user.WalletAddress = dto.WalletAddress.ToLower(); ;

            await _db.SaveChangesAsync();   

            return Ok();
        }
    }
}
