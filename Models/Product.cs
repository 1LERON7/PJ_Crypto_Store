using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class product
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime Created { get; set; }

    public virtual ICollection<order_item> OrderItems { get; set; } = new List<order_item>();

    public virtual ICollection<user> Users { get; set; } = new List<user>();
}
