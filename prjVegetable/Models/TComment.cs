using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TComment
{
    public int FId { get; set; }

    public int FPersonId { get; set; }

    public int FProductId { get; set; }

    public int FOrderListId { get; set; }

    public string? FComment { get; set; }

    public int FStar { get; set; }

    public DateOnly FCreatedAt { get; set; }
}
