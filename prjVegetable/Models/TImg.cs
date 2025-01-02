using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TImg
{
    public int FImgId { get; set; }

    public int FProductId { get; set; }

    public string FImgName { get; set; } = null!;

    public int FImgOrder { get; set; }

    public DateTime? FImgCreatedAt { get; set; }

    public string FImgEditer { get; set; } = null!;


    public virtual TProduct FProduct { get; set; } = null!;
}
