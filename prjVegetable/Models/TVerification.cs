using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TVerification
{
    public int FId { get; set; }

    public int FPersonId { get; set; }

    public string Ftoken { get; set; } = null!;

    public string FtokenType { get; set; } = null!;

    public DateTime? FtokenSentTime { get; set; }

    public DateTime FexpirationTime { get; set; }

    public bool FisUsed { get; set; }

    public DateTime? FusedTime { get; set; }
}
