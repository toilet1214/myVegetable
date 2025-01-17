using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TFaq
{
    public int FId { get; set; }

    public string FQuestion { get; set; } = null!;

    public string FAnswer { get; set; } = null!;

    public DateTime FCreatedAt { get; set; }

    public DateTime FUpdatedAt { get; set; }
}
