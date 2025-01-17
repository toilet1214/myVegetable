namespace prjVegetable.ViewModels
{
    public class ProductViewModel
    {
        public string txtClassification { get; set; }
        public string category { get; set; }

        public decimal? MinPrice { get; set; } // 最低價格
        public decimal? MaxPrice { get; set; } // 最高價格
    }
}
