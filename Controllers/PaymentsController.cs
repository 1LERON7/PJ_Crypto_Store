using Crypto_Store.DTOs;
using Crypto_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
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

        [Authorize]
        [HttpPost("create/{productId}")]
        public async Task<IActionResult> CreatePay(Guid productId)
        {
            var product = await _db.Products.FindAsync(productId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var payment = new Payment
            {
                UserId = Guid.Parse(userId),
                ProductId = product.Id,
                Amount = product.Price,
                Currency = "ETH",
                Network = "Localhost",
                Status = "created",
                Confirmations = 0,
                Created = DateTime.UtcNow
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                paymentId = payment.Id,
                amount = payment.Amount
            });
        }

        [Authorize]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentsDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var existingPayment = await _db.Payments
                .FirstOrDefaultAsync(p => p.TxHash == dto.TxHash);

            if (existingPayment != null)
            {
                Console.WriteLine("TX already saved");
                return BadRequest("This transaction has already been saved");
            }


            var product = await _db.Products.FindAsync(dto.ProductId);
            if (product == null)
                return BadRequest("Product not found");

            var web3 = new Web3("http://127.0.0.1:7545");

            var receipt = await web3.Eth.Transactions
                .GetTransactionReceipt
                .SendRequestAsync(dto.TxHash);

            if (receipt == null)
            {
                Console.WriteLine("Transaction receipt = NULL!");
                return BadRequest("Transaction not mined yet");
            }
                

            if (receipt.Status.Value == 0)
            {
                Console.WriteLine("receipt.Status.Value == 0");
                return BadRequest("Transaction failed");
            }
                

            var tx = await web3.Eth.Transactions
                .GetTransactionByHash
                .SendRequestAsync(dto.TxHash);

            if (tx == null)
            {
                Console.WriteLine("Transaction TX = NULL!");
                return BadRequest("Transaction not found");
            }
                

            var valueInEth = Web3.Convert.FromWei(tx.Value.Value);

            if (tx.Value.Value != Web3.Convert.ToWei(product.Price))
            {
                Console.WriteLine("Incorrect amount");
                return BadRequest("This transaction has already been saved");
            }


            var payment = new Payment
            {
                UserId = userId,
                ProductId = dto.ProductId,
                TxHash = dto.TxHash,
                Amount = valueInEth,
                Currency = "ETH",
                Network = "Localhost",
                Status = "confirmed",
                Confirmations = 1,
                Created = DateTime.UtcNow,
                ConfirmedAt = DateTime.UtcNow
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Payment confirmed",
                paymentId = payment.Id
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPayments(
            int page = 1,
            int pageSize = 12,
            string? search = null){

            var query = _db.Payments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(pay => pay.User.Email.Contains(search) || pay.TxHash.Contains(search));

            // TODO: сумма за все время
            var totalAmount = await _db.Payments.SumAsync(p => p.Amount);
            // TODO: средняя сумма
            var averageAmount = await _db.Payments.AverageAsync(p => p.Amount);

            var totalCount = await query.CountAsync();

            var payments = await query
                .OrderByDescending(p => p.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p=> new PaymentsAdminDto
                {
                    Id = p.Id,
                    UserEmail = p.User.Email,
                    ProductTitle = p.Product.Title,
                    Amount = p.Amount,
                    TxHash = p.TxHash,
                    Created = p.Created
                }).ToListAsync();

            return Ok(new
            {
                items = payments,
                totalCount,
                totalAmount,
                averageAmount
            });
        }
    }
}