using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? Created { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
