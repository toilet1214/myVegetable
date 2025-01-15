using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPaymentReversalDetail
{
    public int FId { get; set; }

    public int FPaymentReversalId { get; set; }

    public int FGoodsInAndOutId { get; set; }
}
