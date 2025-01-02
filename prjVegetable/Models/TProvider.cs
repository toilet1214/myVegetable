using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TProvider
{
    public int FId { get; set; }

    public string FName { get; set; } = null!;

    public string FUbn { get; set; } = null!;

    public string FTel { get; set; } = null!;

    public string FConnect { get; set; } = null!;

    public string FAccountant { get; set; } = null!;

    public string FAddress { get; set; } = null!;

    public string FDelivery { get; set; } = null!;

    public string FInvoiceadd { get; set; } = null!;

    public int FEditor { get; set; }
}
