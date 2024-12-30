using System;
using System.Collections.Generic;
using System.Linq;

namespace prjVegetable.Models;

public partial class TCart
{
    public int FId { get; set; }

    public int FProductId { get; set; }

    public string FProductName { get; set; } = null!;

    public int FCount { get; set; }

    public int FPrice { get; set; }

    public int FImgId { get; set; }

    public int FBuyerId { get; set; }
    public decimal TotalPrice => FCount * FPrice; // 總價 (計算屬性)
}
