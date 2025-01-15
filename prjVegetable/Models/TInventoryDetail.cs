using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInventoryDetail
{
    public int FId { get; set; }

    public int FProductId { get; set; }

    public int FSystemQuantity { get; set; }

    public int FActualQuantity { get; set; }

    public string FNote { get; set; } = null!;
}
