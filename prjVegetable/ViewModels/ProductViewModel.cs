using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class ProductViewModel
    {
        public IEnumerable<CProductWrap> Products { get; set; }
        public string txtClassification { get; set; }
        public string category { get; set; }
        public string keyword { get; set; }

        public decimal? MinPrice { get; set; } // 最低價格
        public decimal? MaxPrice { get; set; } // 最高價格

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }


    }
}
