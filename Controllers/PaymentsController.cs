using Crypto_Store.DTOs;
using Crypto_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Crypto_Store.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PaymentsController(AppDbContext db)
        {
            _db = db;
        }

        [Authorize]                 // TODO
        [HttpPost]
        public async Task<IActionResult> Pay(PaymentsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == dto.OrderId && o.UserId == Guid.Parse(userId));

            if (order == null)
                return NotFound();

            if (order.Status == "paid")
                return BadRequest("Order already paid!");

            var payment = new Payment()
            {
                OrderId = order.Id,
                Amount = order.TotalPrice,
                Currency = "ETH",
                Network = "Sepolia", // или Mainnet
                Status = "created", 
                Confirmations = 0,
                Created = DateTime.UtcNow
            };

            _db.Add(payment);

            order.Status = "paid";

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Payment seccess",
                paymentId = payment.Id,
                orderId = order.Id
            });
        }
    }
}
