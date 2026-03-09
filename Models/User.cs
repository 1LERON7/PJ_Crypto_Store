using System;
using System.Collections.Generic;

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

    public string? Username { get; set; }

    public string? Bio { get; set; }

    public string? GamerTag { get; set; }

    public string? WalletAddress { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
