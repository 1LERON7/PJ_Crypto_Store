using System;
using System.Collections.Generic;

namespace Crypto_Store.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int PriceEth { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime? Created { get; set; }
}
