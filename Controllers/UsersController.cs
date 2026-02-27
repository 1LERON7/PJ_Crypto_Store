using Crypto_Store.DTOs;
using Crypto_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Crypto_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public UsersController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            // проверка на пользователя в бд
            var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return BadRequest("User already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,    // ХЭЭШ!!!  закодированный пароль + Соль.
                Role = "user"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new ResponseUserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            });

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(RegisterUserDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) 
                return Unauthorized("Invalid email or password");

            // проверка пароля из запроса и захэшированого из БД.
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
                return Unauthorized("Invalid email or password");


            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
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
                user.Id,
                user.Email,
                user.Role
            });
        }

        


    }
}
