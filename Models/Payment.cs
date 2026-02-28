using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class payment
{
    public Guid id { get; set; }

    public Guid order_id { get; set; }

    public string? tx_hash { get; set; }

    public string status { get; set; } = null!;

    public DateTime created { get; set; } = DateTime.UtcNow;

    public decimal amount { get; set; }

    public virtual order order { get; set; } = null!;
}
