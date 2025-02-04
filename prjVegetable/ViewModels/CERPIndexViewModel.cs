using prjVegetable.Models;

namespace prjVegetable.ViewModels
{
    public class CERPIndexViewModel
    {
        public int TotalMembers { get; set; }
        public List<int> TotalMembersAll { get; set; }
        public List<int> TotalMembersYear { get; set; }
        public List<int> TotalMembersMonth { get; set; }
        public List<string> AllMembersLabels { get; set; }
        public int TotalVisitorsYear { get; set; }
        public int TotalVisitorsMonth { get; set; }
        public int TotalVisitorsDay { get; set; }
        public int TotalOrdersYear { get; set; }
        public int TotalOrdersMonth { get; set; }
        public int TotalOrdersDay { get; set; }
        public List<string> BestSellingProductYear { get; set; }
        public List<string> MostPopularProductYear { get; set; }
        public List<string> BestSellingClassYear { get; set; }
        public List<string> BestSellingProductMonth { get; set; }
        public List<string> MostPopularProductMonth { get; set; }
        public List<string> BestSellingClassMonth { get; set; }
        public List<string> BestSellingProductDay { get; set; }
        public List<string> MostPopularProductDay { get; set; }
        public List<string> BestSellingClassDay { get; set; }

        public List<int> SellingChangeDay { get; set; }
        public List<int> SellingChangeMonth { get; set; }
        public List<int> SellingChangeYear { get; set; }

        public List<int> MemberChangeDay { get; set; }
        public List<int> MemberChangeMonth { get; set; }
        public List<int> MemberChangeYear { get; set; }
    }
}
