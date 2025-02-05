﻿using prjVegetable.Models;

namespace prjVegetable.ViewModels
{

    public class CERPIndexViewModel
    {
        public int TotalMembers { get; set; }
        public List<int> TotalMembersAll { get; set; }
        public List<int> TotalMembersYear { get; set; }
        public List<int> TotalMembersMonth { get; set; }
        public List<int> AllMembersLabels { get; set; }
        public int TotalVisitorsYear { get; set; }
        public int TotalVisitorsMonth { get; set; }
        public int TotalVisitorsDay { get; set; }
        public int TotalOrdersYear { get; set; }
        public int TotalOrdersMonth { get; set; }
        public int TotalOrdersDay { get; set; }
        public IEnumerable<dynamic> BestSellingProductYear { get; set; }
        public List<string> MostPopularProductYear { get; set; }
        public List<int> SellingClassYear { get; set; }
        public IEnumerable<dynamic> BestSellingProductMonth { get; set; }
        public List<string> MostPopularProductMonth { get; set; }
        public List<int> SellingClassMonth { get; set; }
        public IEnumerable<dynamic> BestSellingProductDay { get; set; }
        public List<string> MostPopularProductDay { get; set; }
        public List<int> SellingClassDay { get; set; }
    }
}
