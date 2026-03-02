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
        [HttpPost("/create")]
        public async Task<IActionResult> CreateOrder(CreateOrdersDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var products = await _db.products.Where(p=> dto.ProductIds.Contains(p.id)).ToListAsync();
            if (products.Count != dto.ProductIds.Count)
                return BadRequest("One or more products not found");

            var totalPrice = products.Sum(p=> p.price);

            var order = new order
            {
                user_id = Guid.Parse(userId),
                total_price = totalPrice,
                status = "created"

            };

            _db.orders.Add(order);
            await _db.SaveChangesAsync();

            var orderItems = products.Select(p => new order_item
            {
                order_id = order.id,
                product_id = p.id,
                price = totalPrice,
                quantity = products.Count()
            });

            // AddRange(); -- добавления массива значений в БД
            _db.order_items.AddRange(orderItems);
            await _db.SaveChangesAsync();

            return Ok(order);
        }

        [Authorize]
        [HttpGet("/my")]
        public async Task<IActionResult> GetMyOrders(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null) 
                return Unauthorized();

            var orders = await _db.orders.Where(o => o.user_id == Guid.Parse(userId)).ToListAsync();

            
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var order = await _db.orders.FirstOrDefaultAsync(o=> o.id == id && o.user_id == Guid.Parse(userId));

            if(order == null) 
                return NotFound();

            return Ok(order);   
        }

    }
}
