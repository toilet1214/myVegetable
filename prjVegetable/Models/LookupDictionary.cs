using System.Collections.Generic;

namespace prjVegetable.Models
{
    public static class LookupDictionary
    {
        // 進出貨狀態 (0 = 進貨, 1 = 出貨)
        public static readonly Dictionary<int, string> InOutMapping = new Dictionary<int, string>
        {
            { 0, "進貨" },
            { 1, "出貨" },
            { 2, "廢棄貨單(已取消)" }
        };

        // 訂單狀態 (fStatus)
        public static readonly Dictionary<int, string> StatusMapping = new Dictionary<int, string>
        {
            { 0, "處理中" },
            { 1, "已確認" },
            { 2, "已完成" },
            { 3, "已取消" }
        };

        // 付款狀態 (fPay)
        public static readonly Dictionary<int, string> PayStatusMapping = new Dictionary<int, string>
        {
            { 0, "未付款" },
            { 1, "付款失敗" },
            { 2, "付款成功" }
        };

    }
}
