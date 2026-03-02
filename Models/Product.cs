using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Models;

[Table("products")]
public partial class Product
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("title")]
    [StringLength(50)]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("price")]
    [Precision(18, 8)]
    public decimal Price { get; set; }

    [Column("image_url")]
    public string ImageUrl { get; set; } = null!;

    [Column("created")]
    public DateTime Created { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
