using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TProduct
{
    public int FId { get; set; }

    public string FName { get; set; } = null!;

    public string FClassification { get; set; } = null!;

    public int FPrice { get; set; }

    public string FDescription { get; set; } = null!;

    public int FQuantity { get; set; }

    public DateTime FLaunchAt { get; set; }

    public string FStorage { get; set; } = null!;

    public string FOrigin { get; set; } = null!;

    public string FEditer { get; set; } = null!;
}
