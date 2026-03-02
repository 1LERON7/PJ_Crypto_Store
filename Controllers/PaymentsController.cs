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

            var order = await _db.orders.FirstOrDefaultAsync(o => o.id == dto.OrderId && o.user_id == Guid.Parse(userId));

            if (order == null)
                return NotFound();

            if (order.status == "paid")
                return BadRequest("Order already paid!");

            var payment = new payment()
            {
                order_id = order.id,
                amount = order.total_price,
                // PaymentMethod = "test",      ADD TO DB
                status = "confirmed",
                created = DateTime.Now
            };

            _db.Add(payment);

            order.status = "paid";

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Payment seccess",
                paymentId = payment.id,
                orderId = order.id
            });
        }
    }
}
