using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInventoryMain
{
    public int FId { get; set; }

    public DateOnly FBaselineDate { get; set; }

    public DateOnly FCreatedAt { get; set; }

    public int FEditor { get; set; }

    public string FNote { get; set; } = null!;
}
