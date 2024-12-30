﻿using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPerson
{
    public int FId { get; set; }

    public string FName { get; set; } = null!;

    public string FAccount { get; set; } = null!;

    public string FPassword { get; set; } = null!;

    public string FGender { get; set; } = null!;

    public DateOnly FBirth { get; set; }

    public string FPhone { get; set; } = null!;

    public string FTel { get; set; } = null!;

    public string FAddress { get; set; } = null!;

    public string FEmail { get; set; } = null!;

    public string FUbn { get; set; } = null!;

    public string FPermissiion { get; set; } = null!;

    public string FEmp { get; set; } = null!;

    public string FTelEmptel { get; set; } = null!;

    public DateTime? FCreatedAt { get; set; }

    public string FEditor { get; set; } = null!;
}
