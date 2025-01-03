using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInventoryDetail
{
    public int FId { get; set; }

    public string FInventoryNo { get; set; } = null!;

    public int FProductId { get; set; }

    public string FProductName { get; set; } = null!;

    public int FSystemQuantity { get; set; }

    public int? FActualQuantity { get; set; }

    public int? FDifferenceQuantity { get; set; }

    public int FAmount { get; set; }

    public string? FRemark { get; set; }

    public string? FEditorId { get; set; }
}
