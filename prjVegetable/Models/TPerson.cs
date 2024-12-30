using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPerson
{
    public int FPId { get; set; }

    public string FPName { get; set; }

    public string FPAccount { get; set; }

    public string FPPassword { get; set; }

    public string FPGender { get; set; }

    public DateOnly FPBirth { get; set; }

    public string FPPhone { get; set; }

    public string FPTel { get; set; }

    public string FPAddress { get; set; }

    public string FPEmail { get; set; }

    public string FPUinvoice { get; set; }

    public string FPStatus { get; set; }

    public string FPEmp { get; set; }

    public string FPTelEmptel { get; set; }

    public DateTime? FPCreatedAt { get; set; }

    public string? FPEditor { get; set; } 
}
