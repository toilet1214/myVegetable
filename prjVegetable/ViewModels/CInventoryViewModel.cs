using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class CInventoryViewModel
    {
        public IEnumerable<CInventoryDetailWrap> InventoryDetails { get; set; }
        public CInventoryMainWrap InventoryMain { get; set; }
        public IEnumerable<CProductUpdateWrap> Products { get; set; }


        public int TotalItemCount { get; set; }
        public int CurrentItemCount { get; set; }


        public class CProductUpdateWrap
        {
            public int FId { get; set; }
            public string? FName { get; set; }
            public int FQuantity { get; set; }
        }
    }
}
