using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Models;

[Table("users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    
    [Required]
    [Column("email")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Column("role")]
    public string Role { get; set; } = null!;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("created")]
    public DateTime Created { get; set; }

    [Column("refresh_token")]
    public string? RefreshToken { get; set; }

    [Column("refresh_token_time")]
    public DateTime? RefreshTokenTime { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
