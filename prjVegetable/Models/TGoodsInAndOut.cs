using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TGoodsInAndOut
{
    public int FId { get; set; }

    public int FInOut { get; set; }

    public DateTime FDate { get; set; }

    public int FInvoiceId { get; set; }

    public int FProviderId { get; set; }

    public int FPersonId { get; set; }

    public int FProductId { get; set; }

    public int? FCount { get; set; } = 0;

    public int? FPrice { get; set; } = 0;

    public int? FTotal { get; set; } = 0;

    public int FEditor { get; set; }

    public string? FNote { get; set; } 
}
