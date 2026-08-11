using Crypto_Store.Models;

namespace Crypto_Store.DTOs
{
    public class OrderDto
    {
        public Guid? UserId { get; set; }

        public int TotalPrice { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual User? User { get; set; }
    }
}
