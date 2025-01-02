using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInvoiceDetail
{
    public int FId { get; set; }

    public string? FNumber { get; set; }

    public string? FProductName { get; set; }

    public int? FConut { get; set; }

    public int? FPrice { get; set; }

    public int? FSum { get; set; }
}
