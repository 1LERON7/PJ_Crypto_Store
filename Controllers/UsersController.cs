using Crypto_Store.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Crypto_Store.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            // проверка на пользователя в бд
            var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return BadRequest("User already exists");

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = dto.Password,    // ХЭЭШ!!!
                Role = "user"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new { user.Id, user.Email });

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetUsers()
        //{
        //    return Ok();
        //}

        //[HttpGet("id")]
        //public async Task<IActionResult> GetUsersById()
        //{
        //    return Ok();
        //}


    }
}
