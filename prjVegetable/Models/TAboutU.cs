using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TAboutU
{
    public int FId { get; set; }

    public string FTitle { get; set; } = null!;

    public string FContent { get; set; } = null!;

    public DateTime FCreatedAt { get; set; }

    public DateTime FUpdatedAt { get; set; }
}
