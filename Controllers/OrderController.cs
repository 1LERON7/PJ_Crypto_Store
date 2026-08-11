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
using System.Threading.Tasks;

namespace Crypto_Store.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrderController(AppDbContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(CreateOrdersDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var products = await _db.Products.Where(p=> dto.ProductIds.Contains(p.Id)).ToListAsync();
            if (products.Count != dto.ProductIds.Count)
                return BadRequest("One or more products not found");

            var totalPrice = products.Sum(p=> p.Price);

            var order = new Order
            {
                UserId = Guid.Parse(userId),
                TotalPrice = totalPrice,
                Status = "created"

            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            var orderItems = products.Select(p => new OrderItem
            {
                OrderId = order.Id,
                ProductId = p.Id,
                Price = totalPrice,
                Quantity = products.Count()
            });

            // AddRange(); -- добавления массива значений в БД
            _db.OrderItems.AddRange(orderItems);
            await _db.SaveChangesAsync();

            return Ok(order);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null) 
                return Unauthorized();

            var orders = await _db.Orders.Where(o => o.UserId == Guid.Parse(userId)).ToListAsync();

            
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var order = await _db.Orders.FirstOrDefaultAsync(o=> o.Id == id && o.UserId == Guid.Parse(userId));

            if(order == null) 
                return NotFound();

            return Ok(order);   
        }

    }
}
