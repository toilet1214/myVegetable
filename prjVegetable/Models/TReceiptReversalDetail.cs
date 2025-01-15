using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TReceiptReversalDetail
{
    public int FId { get; set; }

    public int FReceiptReversalId { get; set; }

    public int FOrderId { get; set; }
}
