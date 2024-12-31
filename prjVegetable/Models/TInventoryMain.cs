using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInventoryMain
{
    public int FId { get; set; }

    public string FInventoryNo { get; set; } = null!;

    public DateOnly FBaselineDate { get; set; }

    public DateTime FCreatedAt { get; set; }

    public string FCreatorId { get; set; } = null!;

    public string? FEditor { get; set; }
}
