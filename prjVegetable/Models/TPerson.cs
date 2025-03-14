﻿using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPerson
{
    public int FId { get; set; }

    public string FName { get; set; } = null!;

    public string FAccount { get; set; } = null!;

    public string? FPassword { get; set; }

    public string FGender { get; set; } = null!;

    public DateOnly FBirth { get; set; }

    public string FPhone { get; set; } = null!;

    public string FTel { get; set; } = null!;

    public string FAddress { get; set; } = null!;

    public string? FUbn { get; set; }

    public int FPermission { get; set; }

    public string? FEmp { get; set; }

    public string? FEmpTel { get; set; }

    public DateTime FCreatedAt { get; set; }

    public int? FEditor { get; set; }

    public bool FIsVerified { get; set; }

    public int FLoginType { get; set; }

    public string? FGoogleId { get; set; }
}
