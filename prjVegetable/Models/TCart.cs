using System;
using System.Collections.Generic;

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

    // 新增 TotalPrice 屬性
    public int TotalPrice
    {
        get
        {
            return FPrice * FCount;
        }
    }

}
