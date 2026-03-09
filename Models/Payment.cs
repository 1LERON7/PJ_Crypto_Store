using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;
//public enum PaymentStatus
//{
//    created,
//    pending,
//    confirmed,
//    failed
//}

public partial class Payment
{
    public Guid Id { get; set; }

    public string? TxHash { get; set; }

    public string Currency { get; set; } = null!;

    public string? Network { get; set; }
    
    public int? Confirmations { get; set; }

    public DateTime? ConfirmedAt { get; set; }
    public string Status { get; set; }

    public DateTime Created { get; set; }

    public decimal Amount { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ProductId { get; set; }
    public Product Product { get; set; }

    public virtual User? User { get; set; }
}
