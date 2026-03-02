using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Models;

[Table("payments")]
public partial class Payment
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Column("tx_hash")]
    public string? TxHash { get; set; }

    [Column("currency")]
    public string Currency { get; set; } = null!;

    [Column("network")]
    public string? Network { get; set; }

    [Column("confirmations")]
    public int? Confirmations { get; set; }

    [Column("confirmed_at", TypeName = "timestamp without time zone")]
    public DateTime? ConfirmedAt { get; set; }

    [Column("status")]
    public string Status { get; set; } = null!;

    [Column("created", TypeName = "timestamp without time zone")]
    public DateTime Created { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payments")]
    public virtual Order Order { get; set; } = null!;
}
