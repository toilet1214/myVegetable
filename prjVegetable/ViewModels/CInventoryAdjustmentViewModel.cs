using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class CInventoryAdjustmentViewModel
    {
        public CInventoryAdjustmentWrap InventoryAdjustment { get; set; }
        public IEnumerable<CInventoryAdjustmentDetailWrap> InventoryAdjustmentDetail { get; set; }
        public IEnumerable<CProductUpdateWrap> Products { get; set; }
        public IEnumerable<CInventoryAdjustmentUpdateWrap> AdjustmentUpdate { get; set; }

        public class TGoodsInAndOutDetail
        {
            public int FProductId { get; set; }
            public int FPrice { get; set; }
        }
        public class CProductUpdateWrap
        {
            public int FId { get; set; }
            public string? FName { get; set; }
            public int FQuantity { get; set; }
        }
        public class CInventoryAdjustmentUpdateWrap
        {
            public int FId { get; set; }
            public string FNote { get; set; }
        }

    }
}
