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

        [Authorize]                 // TODO
        [HttpPost("create/{orderId}")]
        public async Task<IActionResult> CreatePay(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalPrice,
                Currency = "ETH",
                Network = "Sepolia",
                Status = "created",
                Confirmations = 0,
                Created = DateTime.UtcNow
            };

            _db.Payments.Add(payment);

            //order.Status = "paid";

            await _db.SaveChangesAsync();

            return Ok(new
            {
                paymentId = payment.Id,
                amount = payment.Amount
            });
        }
    
    [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentsDto dto)
        {
            var payment = await _db.Payments.FindAsync(dto.PaymentId);

            if (payment == null) return BadRequest("Payment not found");


            // подключение Ethereum-сети через RPC
            var web3 = new Web3("https://sepolia.infura.io/v3/YOUR_INFURA_KEY");


            // подтверждение транзакции
            TransactionReceipt receipt = await web3.Eth.Transactions
                .GetTransactionReceipt.SendRequestAsync(dto.TxHash);

            if (receipt == null)
                return BadRequest("Transaction not mined yet");

            if (receipt.Status.Value == 0)
                return BadRequest("Transaction failed");

            // все детали успешной транзакции
            var tx = await web3.Eth.Transactions
                .GetTransactionByHash
                .SendRequestAsync(dto.TxHash);

            decimal valueInEth = Web3.Convert.FromWei(tx.Value.Value);

            if (tx.Value.Value != Web3.Convert.ToWei(payment.Amount))
                return BadRequest("Incorrect payment amount");


            payment.Status = "confirmed";
            payment.TxHash = dto.TxHash;
            payment.ConfirmedAt = DateTime.UtcNow;

            var order = await _db.Orders.FindAsync(payment.OrderId);
            order.Status = "paid";

            await _db.SaveChangesAsync();

            return Ok("Payment confirmed");
        }
    }
}