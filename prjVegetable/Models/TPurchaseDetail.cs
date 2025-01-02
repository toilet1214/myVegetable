using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPurchaseDetail
{
    public int FId { get; set; }

    public string? FPurchaseId { get; set; }

    public string? FProductName { get; set; }

    public int? FConut { get; set; }

    public int? FPrice { get; set; }

    public int? FSum { get; set; }
}
