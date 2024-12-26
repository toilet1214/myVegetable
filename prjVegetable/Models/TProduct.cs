using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjVegetable.Models;

public partial class TProduct
{
    public int FProductId { get; set; }

    public string FProductName { get; set; } = null!;

    public string FProductClassification { get; set; } = null!;

    public int FProductPrice { get; set; }

    public string FProductDescription { get; set; } = null!;

    public int FProductQuantity { get; set; }

    public DateTime FProductCreatedAt { get; set; }

    public string FProductStorage { get; set; } = null!;

    public string FProductOrigin { get; set; } = null!;

    public string FProductEditer { get; set; } = null!;

    public virtual ICollection<TImg> TImgs { get; set; } = new List<TImg>();

    [NotMapped]
    //public TImg TImg { get; set; }
    public string FImgName { get; set; }
}
