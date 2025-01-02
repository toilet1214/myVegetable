using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TImg
{
    public int FId { get; set; }

    public int FProductId { get; set; }

    public string FImgName { get; set; } = null!;

    public int FOrderBy { get; set; }

    public DateTime? FUploadAt { get; set; }

    public string FEditor { get; set; } = null!;
}
