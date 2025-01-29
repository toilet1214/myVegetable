using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInventoryMain
{
    public int FId { get; set; }

    public DateTime FBaselineDate { get; set; }

    public DateTime FCreatedAt { get; set; }

    public int FEditor { get; set; }

    public string? FNote { get; set; }
}
