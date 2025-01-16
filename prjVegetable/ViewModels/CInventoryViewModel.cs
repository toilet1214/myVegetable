using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class CInventoryViewModel
    {
        public IEnumerable<CInventoryDetailWrap> InventoryDetails { get; set; }
        public CInventoryMainWrap InventoryMain { get; set; }

        public IEnumerable<CProductWrap> Products { get; set; }
    }
}
