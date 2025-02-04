using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace prjVegetable.Controllers
{
    public class ERPController : Controller
    {

        public readonly DbVegetableContext _VegetableContext;

        public ERPController(DbVegetableContext VegetableContext)
        {
            _VegetableContext = VegetableContext;
        }

        public IActionResult Index()
        {
            int TotalOrdersYear = _VegetableContext.TOrders.Count(o => o.FOrderAt.Year == DateTime.Now.Year);
            int TotalOrdersMonth = _VegetableContext.TOrders.Count(o => o.FOrderAt.Month == DateTime.Now.Month && o.FOrderAt.Year == DateTime.Now.Year);
            int TotalOrdersDay = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date);
            int TotalOrdersLastYear = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date.AddYears(-1));
            int TotalOrdersLastMonth = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date.AddMonths(-1));
            int TotalOrdersLastDay = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date.AddDays(-1));
            if (TotalOrdersLastYear != 0)
            {
                ViewBag.OrdersPercentageYear = Math.Round(((decimal)(TotalOrdersYear / TotalOrdersLastYear - 1) * 100), 1);
            }
            else ViewBag.OrdersPercentageYear = 100;
            if (TotalOrdersLastMonth != 0)
            {
                ViewBag.OrdersPercentageMonth = Math.Round((decimal)((TotalOrdersMonth / TotalOrdersLastMonth - 1) * 100), 1);
            }
            else ViewBag.OrdersPercentageMonth = 100;
            if (TotalOrdersLastDay != 0)
            {
                ViewBag.OrdersPercentageDay = Math.Round(((decimal)(TotalOrdersDay / TotalOrdersLastDay - 1) * 100), 1);
            }
            else ViewBag.OrdersPercentageDay = 100;

            ViewBag.VisitorsPercentageDay = 100;
            ViewBag.VisitorsPercentageMonth = 80;
            ViewBag.VisitorsPercentageYear = 70;

            List<int> TotalMembersMonth = _VegetableContext.TPeople.Where(p => p.FPermission == 0 && p.FCreatedAt >= DateTime.Now.Date.AddMonths(-1)).GroupBy(p => p.FCreatedAt.Day).OrderBy(g => g.Key).Select(g => g.Count()).ToList();
            for (int i = 1; i < TotalMembersMonth.Count; i++)
            {
                TotalMembersMonth[i] += TotalMembersMonth[i - 1];  // 每一天的數字加上前一天的數字
            }
            while (TotalMembersMonth.Count < 30)
            {
                TotalMembersMonth.Insert(0,0);
            }
            List<int> TotalMembersYear = _VegetableContext.TPeople.Where(p => p.FPermission == 0 && p.FCreatedAt >= DateTime.Now.Date.AddYears(-1)).GroupBy(p => p.FCreatedAt.Month).OrderBy(g => g.Key).Select(g => g.Count()).ToList();
            for (int i = 1; i < TotalMembersYear.Count; i++)
            {
                TotalMembersYear[i] += TotalMembersYear[i - 1];  // 每一天的數字加上前一天的數字
            }
            while (TotalMembersYear.Count < 12)
            {
                TotalMembersYear.Insert(0, 0);
            }
            List<int> TotalMembersAll = _VegetableContext.TPeople.Where(p => p.FPermission == 0).GroupBy(p => p.FCreatedAt.Year).OrderBy(g => g.Key).Select(g => g.Count()).ToList();
            for (int i = 1; i < TotalMembersAll.Count; i++)
            {
                TotalMembersAll[i] += TotalMembersAll[i - 1];  // 每一天的數字加上前一天的數字
            }
            TotalMembersAll.Insert(0, 0);
            var viewmodel = new CERPIndexViewModel
            {
                AllMembersLabels = _VegetableContext.TPeople.Where(p => p.FPermission == 0).GroupBy(m => m.FCreatedAt.Year).Select(g => g.Key.ToString()).ToList(),
                TotalMembers = _VegetableContext.TPeople.Count(p => p.FPermission == 0),
                TotalMembersAll = TotalMembersAll,
                TotalMembersYear = TotalMembersYear,
                TotalMembersMonth = TotalMembersMonth,
                TotalVisitorsYear = 100,//待修改
                TotalVisitorsMonth = 10,//待修改
                TotalVisitorsDay = 1,//待修改
                TotalOrdersYear = TotalOrdersYear,
                TotalOrdersMonth = TotalOrdersMonth,
                TotalOrdersDay = TotalOrdersDay,
                //BestSellingProductYear = ""  ,
                //BestSellingProductMonth ="",
                //BestSellingProductDay ="",

                //MostPopularProductYear ="",
                //MostPopularProductMonth ="",
                //MostPopularProductDay ="",

                //BestSellingClassYear ="",
                //BestSellingClassMonth ="",
                //BestSellingClassDay =""
            };
            return View(viewmodel);
        }
        public IActionResult ProductBuying()
        {
            return View();
        }

        //盤點作業 by允7
        public IActionResult Inventory()
        {
            ViewBag.AddUrl = Url.Action("AddItem", "Inventory");
            var TInventoryDetail = _VegetableContext.TInventoryDetails.ToList();
            return View(TInventoryDetail);
        }

        public IActionResult InventoryAdjustment()
        {
            ViewBag.AddUrl = Url.Action("AddItem", "Inventory");
            var TInventoryDetail = _VegetableContext.TInventoryDetails.ToList();
            return View(TInventoryDetail);
        }

        public IActionResult CardSetting()
        {
            return View();
        }
        public IActionResult ProductSelling()
        {
            return View();
        }

        public IActionResult CarouselSetting()
        {
            return View();
        }
    }
}