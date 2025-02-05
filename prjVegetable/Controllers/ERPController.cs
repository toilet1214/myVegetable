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

            var members = _VegetableContext.TPeople.Where(p => p.FPermission == 0).ToList();
            var startDate = DateTime.Now.Date.AddDays(-29);  // 確保範圍涵蓋完整30天
            // 初始化字典，確保每一天都有數據 (預設為 0)
            Dictionary<string, int> TotalMembersMonth = Enumerable.Range(0, 30)
                .Select(i => startDate.AddDays(i).ToString("MM/dd"))  // 轉成 "月/日" 格式
                .ToDictionary(d => d, d => 0);

            // 取出過去 30 天內的會員數據，按日期分組
            var groupedDataDay = members
                .Where(p => p.FCreatedAt >= startDate)  // 限制在過去 30 天內
                .GroupBy(p => p.FCreatedAt.ToString("MM/dd")) // 按 "月/日" 分組
                .OrderBy(g => g.Key) // 確保按照日期排序
                .ToList();

            // 填入實際數據
            foreach (var g in groupedDataDay)
            {
                TotalMembersMonth[g.Key] = g.Count();
            }

            // **累積計算**
            int cumulativeCountMonth = 0;
            foreach (var key in TotalMembersMonth.Keys.OrderBy(k => k))
            {
                cumulativeCountMonth += TotalMembersMonth[key];  // 累積總數
                TotalMembersMonth[key] = cumulativeCountMonth;
            }
            var startMonth = DateTime.Now.AddMonths(-11); // 往前推 11 個月，加上當月總共 12 個月
            // 初始化字典，確保每個月份都有數據 (預設為 0)
            Dictionary<string, int> TotalMembersYear = Enumerable.Range(0, 12)
                .Select(i => startMonth.AddMonths(i).ToString("yyyy/MM"))
                .ToDictionary(m => m, m => 0);
            // 取出過去 12 個月內的會員數據，按月份分組
            var groupedDataYear = members
                .Where(p => p.FCreatedAt >= startMonth)  // 限制在過去 12 個月內
                .GroupBy(p => p.FCreatedAt.ToString("yyyy/MM")) // 按照"年/月"分組
                .OrderBy(g => g.Key) // 確保按年月排序
                .ToList();
            foreach (var g in groupedDataYear)
            {
                TotalMembersYear[g.Key] = g.Count(); // 記錄該月份的新增會員數
            }
            int cumulativeCountYear = 0;
            foreach (var key in TotalMembersYear.Keys.OrderBy(k => k))
            {
                cumulativeCountYear += TotalMembersYear[key]; // 加上前面的累積數
                TotalMembersYear[key] = cumulativeCountYear;  // 更新為累積總數
            }
            var minYear = members.Select(p => p.FCreatedAt.Year).DefaultIfEmpty(DateTime.Now.Year).Min();
            var memberCounts = members.Where(p => p.FPermission == 0).GroupBy(p => p.FCreatedAt.Year).OrderBy(g => g.Key).ToDictionary(g => g.Key, g => g.Count());
            Dictionary<int, int> TotalMembersAll = new Dictionary<int, int>();
            int cumulativeCount = 0;
            for (int year = Math.Min(minYear, DateTime.Now.Year - 4); year <= DateTime.Now.Year; year++)
            {
                if (memberCounts.ContainsKey(year))
                    cumulativeCount += memberCounts[year]; // 加上該年新註冊的會員數

                TotalMembersAll[year] = cumulativeCount; // 儲存累積會員數
            }

            var BestSellingProductYear = _VegetableContext.TOrderLists.Join(_VegetableContext.TProducts, ol => ol.FProductId, p => p.FId, (ol, p) => new
            { ol, p }).Join(_VegetableContext.TOrders, b => b.ol.FOrderId, o => o.FId, (b, o) => new
            {
                Total = b.ol.FCount,
                TotalSum = b.p.FPrice * b.ol.FCount,
                b.p.FName,
                o.FStatus,
                Time = o.FOrderAt
            }).Where(x => x.FStatus == 2 && x.Time.Year == DateTime.Now.Year).GroupBy(x => x.FName).Select(g => new
            {
                ProductName = g.Key,
                Total = g.Sum(x => x.Total),       // 總銷售量
                TotalSum = g.Sum(x => x.TotalSum) // 總營收
            })
            .OrderByDescending(x => x.Total) // 按銷量排序
            .Take(5)  // 取前 5 名
            .ToList();
            var BestSellingProductMonth = _VegetableContext.TOrderLists.Join(_VegetableContext.TProducts, ol => ol.FProductId, p => p.FId, (ol, p) => new
            { ol, p }).Join(_VegetableContext.TOrders, b => b.ol.FOrderId, o => o.FId, (b, o) => new
            {
                Total = b.ol.FCount,
                TotalSum = b.p.FPrice * b.ol.FCount,
                b.p.FName,
                o.FStatus,
                Time = o.FOrderAt
            }).Where(x => x.FStatus == 2 && x.Time.Year == DateTime.Now.Year && x.Time.Month == DateTime.Now.Month).GroupBy(x => x.FName).Select(g => new
            {
                ProductName = g.Key,
                Total = g.Sum(x => x.Total),       // 總銷售量
                TotalSum = g.Sum(x => x.TotalSum) // 總營收
            });
            var BestSellingProductDay = _VegetableContext.TOrderLists.Join(_VegetableContext.TProducts, ol => ol.FProductId, p => p.FId, (ol, p) => new
            { ol, p }).Join(_VegetableContext.TOrders, b => b.ol.FOrderId, o => o.FId, (b, o) => new
            {
                Total = b.ol.FCount,
                TotalSum = b.p.FPrice * b.ol.FCount,
                b.p.FName,
                o.FStatus,
                Time = o.FOrderAt
            }).Where(x => x.FStatus == 2 && x.Time.Year == DateTime.Now.Year && x.Time.Month == DateTime.Now.Month && x.Time.Date == DateTime.Now.Date).GroupBy(x => x.FName).Select(g => new
            {
                ProductName = g.Key,
                Total = g.Sum(x => x.Total),       // 總銷售量
                TotalSum = g.Sum(x => x.TotalSum) // 總營收
            });

            var MostPopularProductYear = new List<string>();
            var MostPopularProductMonth = new List<string>();
            var MostPopularProductDay = new List<string>();

            List<int> SellingClassYear = _VegetableContext.TOrderLists
                .Join(_VegetableContext.TProducts,
                ol => ol.FProductId,
                p => p.FId,
                (ol, p) => new { ol, p })
                .Join(_VegetableContext.TOrders,
                o => o.ol.FOrderId,
                order => order.FId,
                (o, order) => new
                {
                    Classification = o.p.FClassification,
                    Count = o.ol.FCount,
                    Time = order.FOrderAt,
                    State = order.FStatus
                })
                .Where(t => t.Time.Year == DateTime.Now.Year && t.State == 2)
                .GroupBy(x => x.Classification)
                .Select(g => g.Sum(x => x.Count))
                .ToList();
            List<int> SellingClassMonth = _VegetableContext.TOrderLists
                .Join(_VegetableContext.TProducts,
                ol => ol.FProductId,
                p => p.FId,
                (ol, p) => new { ol, p })
                .Join(_VegetableContext.TOrders,
                o => o.ol.FOrderId,
                order => order.FId,
                (o, order) => new
                {
                    Classification = o.p.FClassification,
                    Count = o.ol.FCount,
                    Time = order.FOrderAt,
                    State = order.FStatus
                })
                .Where(t => t.Time.Month == DateTime.Now.Month && t.Time.Year == DateTime.Now.Year && t.State == 2)
                .GroupBy(x => x.Classification)
                .Select(g => g.Sum(x => x.Count))
                .ToList();
            List<int> SellingClassDay = _VegetableContext.TOrderLists
                .Join(_VegetableContext.TProducts,
                ol => ol.FProductId,
                p => p.FId,
                (ol, p) => new { ol, p })
                .Join(_VegetableContext.TOrders,
                o => o.ol.FOrderId,
                order => order.FId,
                (o, order) => new
                {
                    Classification = o.p.FClassification,
                    Count = o.ol.FCount,
                    Time = order.FOrderAt,
                    State = order.FStatus
                })
                .Where(t => t.Time.Date == DateTime.Now.Date && t.Time.Month == DateTime.Now.Month && t.Time.Year == DateTime.Now.Year && t.State == 2)
                .GroupBy(x => x.Classification)
                .Select(g => g.Sum(x => x.Count))
                .ToList();

            var viewmodel = new CERPIndexViewModel
            {
                AllMembersLabels = TotalMembersAll.Keys.ToList(),
                TotalMembers = _VegetableContext.TPeople.Count(p => p.FPermission == 0),
                TotalMembersAll = TotalMembersAll.Values.ToList(),
                TotalMembersYear = TotalMembersYear.Values.ToList(),
                TotalMembersMonth = TotalMembersMonth.Values.ToList(),
                TotalVisitorsYear = 100,//待修改
                TotalVisitorsMonth = 10,//待修改
                TotalVisitorsDay = 1,//待修改
                TotalOrdersYear = TotalOrdersYear,
                TotalOrdersMonth = TotalOrdersMonth,
                TotalOrdersDay = TotalOrdersDay,
                BestSellingProductYear = BestSellingProductYear,
                BestSellingProductMonth = BestSellingProductMonth,
                BestSellingProductDay = BestSellingProductDay,

                MostPopularProductYear = MostPopularProductYear,
                MostPopularProductMonth = MostPopularProductMonth,
                MostPopularProductDay = MostPopularProductDay,

                SellingClassYear = SellingClassYear,
                SellingClassMonth = SellingClassMonth,
                SellingClassDay = SellingClassDay
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