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

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
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
                Role = "user",
                Created = DateTime.UtcNow
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


            // Создаем рефреш (длительный) токен
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenTime = DateTime.UtcNow.AddDays(7);

            await _db.SaveChangesAsync();

            return Ok(new LoginResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == dto.RefreshToken);

            if (user == null || user.RefreshTokenTime < DateTime.UtcNow)
                return Unauthorized();

            
            // тут новый аксесс токен
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var newToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newToken)
            });
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _db.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
                return Unauthorized();
            user.RefreshToken = null;
            user.RefreshTokenTime = null;

            await _db.SaveChangesAsync();

            return Ok();
        }


    }
}
