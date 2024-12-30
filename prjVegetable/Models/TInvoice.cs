using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInvoice
{
    public int FId { get; set; }

    public string FNumber { get; set; } = null!;

    public DateTime FDate { get; set; }

    public string FForm { get; set; } = null!;

    public int FCustomerId { get; set; }

    public string FCustomerUbn { get; set; } = null!;

    public int FSupplierId { get; set; }

    public string FSupplierUbn { get; set; } = null!;

    public int FInOut { get; set; }

    public int FStatus { get; set; }

    public int FTotal { get; set; }

    public string? FEditer { get; set; }
}
