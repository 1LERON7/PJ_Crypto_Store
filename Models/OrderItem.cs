using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Models;

[Table("order_items")]
[Index("OrderId", Name = "idx_order_items_order_id")]
[Index("ProductId", Name = "idx_order_items_product_id")]
[Index("OrderId", "ProductId", Name = "order_items_order_id_product_id_key", IsUnique = true)]
public partial class OrderItem
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("price")]
    [Precision(18, 8)]
    public decimal Price { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderItems")]
    public virtual product Product { get; set; } = null!;
}
