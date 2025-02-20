using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TVerification
{
    public int FId { get; set; }

    public int FPersonId { get; set; }

    public string FToken { get; set; } = null!;

    public string FTokenType { get; set; } = null!;

    public DateTime? FTokenSentTime { get; set; }

    public DateTime FExpirationTime { get; set; }

    public bool FIsUsed { get; set; }

    public DateTime? FUsedTime { get; set; }
}
