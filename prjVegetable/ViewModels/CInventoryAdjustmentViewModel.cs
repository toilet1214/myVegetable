using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class CInventoryAdjustmentViewModel
    {
        public IEnumerable<CInventoryAdjustmentDetailWrap> InventoryAdjustmentDetail { get; set; }
        public CInventoryAdjustmentWrap InventoryAdjustment { get; set; }
        public IEnumerable<CProductWrap> Products { get; set; }

    }
}
