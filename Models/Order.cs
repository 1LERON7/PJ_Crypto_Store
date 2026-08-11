using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Models;

[Table("orders")]
[Index("UserId", Name = "idx_orders_user_id")]
public partial class Order
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("total_price")]
    [Precision(18, 8)]
    public decimal TotalPrice { get; set; }

    [Column("status")]
    public string Status { get; set; } = null!;

    [Column("created")]
    public DateTime Created { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    //[InverseProperty("Order")]
    //public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}
