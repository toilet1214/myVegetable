using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TOrder
{
    public int FId { get; set; }

    public int FPersonId { get; set; }

    public int FStatus { get; set; }

    public int FPay { get; set; }

    public DateTime FOrderAt { get; set; }

    public int FTotal { get; set; }

    public string FAddress { get; set; } = null!;

    public string FReceiverName { get; set; } = null!;

    public string FPhone { get; set; } = null!;

    public string FNote { get; set; } = null!;

    public string FMerchantTradeNo { get; set; } = null!;
}
