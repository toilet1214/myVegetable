﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjVegetable.Models;

public partial class TPurchase
{
    public int FId { get; set; }

    public DateTime FBuyDate { get; set; }

    public int FProviderId { get; set; }

    public int FInvoiceForm { get; set; }

    public int FPayment { get; set; }

    public int FEditor { get; set; }

    public int FPreTax { get; set; }

    public int FTax { get; set; }

    public int FTotal { get; set; }

    public string FNote { get; set; }
}