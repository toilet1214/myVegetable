using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TImg
{
    public int FId { get; set; }

    public int FProductId { get; set; }

    public string FName { get; set; } = null!;

    public int FOrder { get; set; }

    public DateTime? FUploadAt { get; set; }

    public string FEditer { get; set; } = null!;
}
