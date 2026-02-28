using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class user
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime Created { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public virtual ICollection<order> Orders { get; set; } = new List<order>();

    public virtual ICollection<product> Products { get; set; } = new List<product>();
}
