using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPurchase
{
    public int FId { get; set; }

    public DateTime FBuyDate { get; set; }

    public string FSupplierId { get; set; } = null!;

    public string FSupplierName { get; set; } = null!;

    public string FBuyer { get; set; } = null!;

    public DateTime FExpirationDate { get; set; }

    public bool FIsTax { get; set; }

    public string FInvoiceForm { get; set; } = null!;

    public string FPayment { get; set; } = null!;

    public string FCreater { get; set; } = null!;

    public string FEditor { get; set; } = null!;

    public int FPreTax { get; set; }

    public int FTax { get; set; }

    public int FTotal { get; set; }

    public string FNote { get; set; } = null!;
}
