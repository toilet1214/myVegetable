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

    
    private string _fRemark = "無"; // 預設為 "無"
    public string FRemark
    {
        get => _fRemark;
        set => _fRemark = string.IsNullOrEmpty(value) ? "無" : value; // 如果傳回 null 或空值，設定為 "無"
    }
}
