using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class order_item
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual order Order { get; set; } = null!;

    public virtual product Product { get; set; } = null!;
}
