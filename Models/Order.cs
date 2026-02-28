using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class order
{
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public decimal total_price { get; set; }

    public string status { get; set; } = null!;

    public DateTime created { get; set; } = DateTime.UtcNow;

    public virtual ICollection<order_item> order_items { get; set; } = new List<order_item>();

    public virtual ICollection<payment> payments { get; set; } = new List<payment>();

    public virtual user user { get; set; } = null!;
}
