using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public DateTime Created { get; set; }

    public virtual ICollection<order_item> OrderItems { get; set; } = new List<order_item>();

    public virtual ICollection<payment> Payments { get; set; } = new List<payment>();

    public virtual user User { get; set; } = null!;
}
