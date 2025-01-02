using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TOrder
{
    public int FId { get; set; }

    public int FBuyerId { get; set; }

    public int FTotal { get; set; }

    public string FStatus { get; set; } = null!;

    public DateTime FOrderAt { get; set; }

    public string FAddress { get; set; } = null!;

    public string FReceiverName { get; set; } = null!;

    public string FPhone { get; set; } = null!;

    public string FRemark { get; set; } = null!;
}
