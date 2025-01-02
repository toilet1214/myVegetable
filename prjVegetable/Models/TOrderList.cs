using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TOrderList
{
    public int FId { get; set; }

    public int FOrderId { get; set; }

    public int FProductId { get; set; }

    public string FProductName { get; set; } = null!;

    public int FPrice { get; set; }

    public int? FCount { get; set; }

    public int? FSum { get; set; }
}
