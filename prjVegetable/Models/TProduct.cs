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

    public int FStorage { get; set; }

    public string FOrigin { get; set; } = null!;

    public int FLaunch { get; set; }

    public int FEditor { get; set; }
}
