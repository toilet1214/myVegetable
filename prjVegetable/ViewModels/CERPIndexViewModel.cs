using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class CERPIndexViewModel
    {
        public int TotalMembers { get; set; }
        public int TotalVisitors { get; set; }
        public int TotalOrdersYear { get; set; }
        public string BestSellingProductYear { get; set; }
        public string MostPopularProductYear { get; set; }
        public string BestSellingClassYear { get; set; }
        public int TotalOrdersMonth { get; set; }
        public string BestSellingProductMonth { get; set; }
        public string MostPopularProductMonth { get; set; }
        public string BestSellingClassMonth { get; set; }
        public int TotalOrdersWeek { get; set; }
        public string BestSellingProductWeek { get; set; }
        public string MostPopularProductWeek { get; set; }
        public string BestSellingClassWeek { get; set; }

    }
}
