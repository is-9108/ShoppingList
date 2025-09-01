using System;
using System.Collections.Generic;

namespace ShoppingList.Models;

public partial class ShoppingList
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string ShopName { get; set; } = null!;

    public string ItemName { get; set; } = null!;
}
