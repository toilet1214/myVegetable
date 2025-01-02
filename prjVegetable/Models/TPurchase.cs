using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPurchase
{
    public int FId { get; set; }

    public DateOnly? FBuyDate { get; set; }

    public string? FSupplierId { get; set; }

    public string? FSupplierName { get; set; }

    public string? FBuyer { get; set; }

    public DateOnly? FExpirationDate { get; set; }

    public string? FIsTax { get; set; }

    public string? FInvoiceForm { get; set; }

    public string? FPayment { get; set; }

    public string? FCreater { get; set; }

    public string? FEditor { get; set; }

    public int? FPreTax { get; set; }

    public int? FTax { get; set; }

    public int? FTotal { get; set; }

    public string? FNote { get; set; }
}
