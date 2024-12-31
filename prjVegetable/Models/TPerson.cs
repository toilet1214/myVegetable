using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPerson
{
    public int FId { get; set; }

    public string? FName { get; set; }

    public string FAccount { get; set; } = null!;

    public string FPassword { get; set; } = null!;

    public string FGender { get; set; } = null!;

    public DateOnly FBirth { get; set; }

    public string FPhone { get; set; } = null!;

    public string? FTel { get; set; }

    public string? FAddress { get; set; }

    public string? FEmail { get; set; }

    public string? FUbn { get; set; }

    public string? FPermission { get; set; }

    public string? FEmp { get; set; }

    public string? FEmpTel { get; set; }

    public DateTime? FCreatedAt { get; set; }

    public string? FEditor { get; set; }
}
