using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class product
{
    public Guid id { get; set; }

    public string title { get; set; } = null!;

    public string? description { get; set; }

    public decimal price { get; set; }

    public string image_url { get; set; } = null!;

    public DateTime created { get; set; } = DateTime.UtcNow;

    public virtual ICollection<order_item> order_items { get; set; } = new List<order_item>();

    public virtual ICollection<user> users { get; set; } = new List<user>();
}
