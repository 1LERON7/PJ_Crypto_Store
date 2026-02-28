using Crypto_Store.Models;

namespace Crypto_Store.DTOs
{
    public class OrderDto
    {
        public Guid? UserId { get; set; }

        public int TotalPrice { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<payment> Payments { get; set; } = new List<payment>();

        public virtual user? User { get; set; }
    }
}
