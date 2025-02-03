using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class CERPIndexViewModel
    {
        public int TotalMembers { get; set; }
        public int TotalVisitors { get; set; }
        public int TotalOrdersYear { get; set; }
        public List<string> BestSellingProductYear { get; set; }
        public List<string> MostPopularProductYear { get; set; }
        public List<string> BestSellingClassYear { get; set; }
        public int TotalOrdersMonth { get; set; }
        public List<string> BestSellingProductMonth { get; set; }
        public List<string> MostPopularProductMonth { get; set; }
        public List<string> BestSellingClassMonth { get; set; }
        public int TotalOrdersWeek { get; set; }
        public List<string> BestSellingProductWeek { get; set; }
        public List<string> MostPopularProductWeek { get; set; }
        public List<string> BestSellingClassWeek { get; set; }

    }
}
