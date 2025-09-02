using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingList.Models;

public partial class ShoppingList
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    [Display(Name = "店名")]
    public string ShopName { get; set; } = null!;

    [Display(Name = "商品名")]
    public string ItemName { get; set; } = null!;
}
