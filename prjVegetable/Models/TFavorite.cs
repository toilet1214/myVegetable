using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TFavorite
{
    public int FId { get; set; }

    public int FProductId { get; set; }

    public int FPersonId { get; set; }
}
