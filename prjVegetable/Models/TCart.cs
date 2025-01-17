using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TCart
{
    public int FId { get; set; }

    public int FProductId { get; set; }

    public int FCount { get; set; }

    public int FPersonId { get; set; }
}
