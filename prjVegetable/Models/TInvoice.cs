using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInvoice
{
    public int FId { get; set; }

    public string FInvoiceNumber { get; set; } = null!;

    public int FDate { get; set; }

    public string FForm { get; set; } = null!;

    public string FCustomerId { get; set; } = null!;

    public string FCustomerNumber { get; set; } = null!;

    public string FSupplierId { get; set; } = null!;

    public string FSupplierNumber { get; set; } = null!;

    public string FInOut { get; set; } = null!;

    public string FStatus { get; set; } = null!;

    public int FTotal { get; set; }
}
