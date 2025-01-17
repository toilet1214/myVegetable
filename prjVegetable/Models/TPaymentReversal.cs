using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPaymentReversal
{
    public int FId { get; set; }

    public DateOnly FDate { get; set; }

    public int? FProviderId { get; set; }

    public int FPaymentMethod { get; set; }

    public int FTotal { get; set; }

    public int FEditor { get; set; }

    public string FNote { get; set; } = null!;
}
