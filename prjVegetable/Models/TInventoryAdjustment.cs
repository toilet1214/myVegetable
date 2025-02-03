using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInventoryAdjustment
{
    public int FId { get; set; }

    public DateOnly FadjustmentDate { get; set; }

    public DateOnly FCreatedAt { get; set; }

    public int FEditor { get; set; }

    public string FNote { get; set; } = null!;

    public int? FCheckerId { get; set; }
}
