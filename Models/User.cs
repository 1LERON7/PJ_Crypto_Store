using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class user
{
    public Guid id { get; set; }

    public string email { get; set; } = null!;

    public string role { get; set; } = null!;

    public string password_hash { get; set; } = null!;

    public DateTime created { get; set; } = DateTime.UtcNow;

    public string? refresh_token { get; set; }

    public DateTime? refresh_token_time { get; set; } = DateTime.UtcNow;

    public virtual ICollection<order> orders { get; set; } = new List<order>();

    public virtual ICollection<product> products { get; set; } = new List<product>();
}
