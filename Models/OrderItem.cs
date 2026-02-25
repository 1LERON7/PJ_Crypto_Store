using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class OrderItem
{
    public Guid? OrderId { get; set; }

    public Guid? ProductId { get; set; }

    public int PriceEth { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
