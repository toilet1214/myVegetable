﻿using System;
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
    public int TotalPrice
    {
        get
        {
            return FPrice * FCount; // 總價格 = 單價 * 數量
        }
    }
}
