﻿using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TReceiptReversal
{
    public int FId { get; set; }

    public DateOnly FDate { get; set; }

    public int FPersonId { get; set; }

    public int FReceiptMethod { get; set; }

    public int FTotal { get; set; }

    public int FEditor { get; set; }

    public string FNote { get; set; } = null!;
}
