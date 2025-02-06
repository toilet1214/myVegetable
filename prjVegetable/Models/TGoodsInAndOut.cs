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

    public int FTotal { get; set; }

    public int FEditor { get; set; }

    public string FNote { get; set; } = null!;
}
