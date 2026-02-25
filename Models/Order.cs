using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public int TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
