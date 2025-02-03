using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TReport
{
    public int FId { get; set; }

    public int FPersonId { get; set; }

    public string FClass { get; set; } = null!;

    public string FTitle { get; set; } = null!;

    public string FDescription { get; set; } = null!;

    public DateTime FTime { get; set; }
}
