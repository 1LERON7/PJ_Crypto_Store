using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public string TxHash { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
