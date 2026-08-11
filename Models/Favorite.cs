using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Models;

[Table("favorites")]
[Index("UserId", "ProductId", Name = "favorites_user_id_product_id_key", IsUnique = true)]
public partial class Favorite
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("created", TypeName = "timestamp without time zone")]
    public DateTime Created { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Favorites")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Favorites")]
    public virtual User User { get; set; } = null!;
}
