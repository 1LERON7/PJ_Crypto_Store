using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crypto_Store.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime Created { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenTime { get; set; }

    [StringLength(20)]
    public string? Username { get; set; }

    [StringLength(300)]
    public string? Bio { get; set; }
    [StringLength(35)]
    public string? GamerTag { get; set; }

    public string? WalletAddress { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
