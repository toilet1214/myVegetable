using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInventoryAdjustmentDetail
{
    public int FId { get; set; }

    public int FInventoryAdjustmentId { get; set; }

    public int FProductId { get; set; }

    public int FQuantity { get; set; }

    public decimal FCost { get; set; }
}
