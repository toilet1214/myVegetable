using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TImg
{
    public int FId { get; set; }

    public int FProductId { get; set; }

    public string FName { get; set; } = null!;

    public int FOrderBy { get; set; }

    public DateOnly FUploadAt { get; set; }

    public int FEditor { get; set; }
}
