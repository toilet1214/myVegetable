using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TInvoice
{
    public int FId { get; set; }

    public string? FNumber { get; set; }

    public DateTime? FDate { get; set; }

    public string? FForm { get; set; }

    public int? FCustomerId { get; set; }

    public string? FCustomerUbn { get; set; }

    public int? FProviderId { get; set; }

    public string FProviderUbn { get; set; } = null!;

    public int? FInOut { get; set; }

    public int? FStatus { get; set; }

    public int? FTotal { get; set; }

    public int? FEditor { get; set; }
}
