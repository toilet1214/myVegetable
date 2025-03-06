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
            int TotalOrdersYear = _VegetableContext.TOrders.Count(o => o.FOrderAt.Year == DateTime.Now.Year && o.FStatus == 2);
            int TotalOrdersMonth = _VegetableContext.TOrders.Count(o => o.FOrderAt.Month == DateTime.Now.Month && o.FOrderAt.Year == DateTime.Now.Year && o.FStatus == 2);
            int TotalOrdersDay = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date && o.FStatus == 2);
            int TotalOrdersLastYear = _VegetableContext.TOrders.Count(o => o.FOrderAt.Year == DateTime.Now.Year - 1 && o.FStatus == 2);
            int TotalOrdersLastMonth = _VegetableContext.TOrders.Count(o => o.FOrderAt.Month == DateTime.Now.AddMonths(-1).Month && o.FOrderAt.Year == DateTime.Now.AddMonths(-1).Year && o.FStatus == 2);
            int TotalOrdersLastDay = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date.AddDays(-1) && o.FStatus == 2);

            // 當年與去年比較
            if (TotalOrdersLastYear == 0 && TotalOrdersYear == 0)
            {
                ViewBag.OrdersPercentageYear = 0; // 如果今年和去年都是零
            }
            else if (TotalOrdersLastYear != 0)
            {
                ViewBag.OrdersPercentageYear = Math.Round(((decimal)(TotalOrdersYear / TotalOrdersLastYear - 1) * 100), 1);
            }
            else
            {
                ViewBag.OrdersPercentageYear = 100; // 如果去年沒有訂單，但今年有訂單
            }

            // 當月與上月比較
            if (TotalOrdersLastMonth == 0 && TotalOrdersMonth == 0)
            {
                ViewBag.OrdersPercentageMonth = 0; // 如果本月和上月都是零
            }
            else if (TotalOrdersLastMonth != 0)
            {
                ViewBag.OrdersPercentageMonth = Math.Round((decimal)((TotalOrdersMonth / TotalOrdersLastMonth - 1) * 100), 1);
            }
            else
            {
                ViewBag.OrdersPercentageMonth = 100; // 如果上月沒有訂單，但本月有訂單
            }

            // 當日與昨日比較
            if (TotalOrdersLastDay == 0 && TotalOrdersDay == 0)
            {
                ViewBag.OrdersPercentageDay = 0; // 如果今天和昨天都是零
            }
            else if (TotalOrdersLastDay != 0)
            {
                ViewBag.OrdersPercentageDay = Math.Round(((decimal)(TotalOrdersDay / TotalOrdersLastDay - 1) * 100), 1);
            }
            else
            {
                ViewBag.OrdersPercentageDay = 100; // 如果昨天沒有訂單，但今天有訂單
            }


            double AvgOrderTotalsYear = Math.Round(
                _VegetableContext.TOrders
                    .Where(o => o.FOrderAt.Year == DateTime.Now.Year && o.FStatus == 2)
                    .Select(a => (double?)a.FTotal)
                    .Average() ?? 0, 2);

            double AvgOrderTotalsMonth = Math.Round(
                _VegetableContext.TOrders
                    .Where(o => o.FOrderAt.Month == DateTime.Now.Month && o.FOrderAt.Year == DateTime.Now.Year && o.FStatus == 2)
                    .Select(a => (double?)a.FTotal)
                    .Average() ?? 0, 2);

            double AvgOrderTotalsDay = Math.Round(
                _VegetableContext.TOrders
                    .Where(o => o.FOrderAt.Date == DateTime.Now.Date && o.FStatus == 2)
                    .Select(a => (double?)a.FTotal)
                    .Average() ?? 0, 2);

            // 修正：正確比較去年的平均訂單金額
            double AvgOrderTotalsLastYear = Math.Round(
                _VegetableContext.TOrders
                    .Where(o => o.FOrderAt.Year == DateTime.Now.Year - 1 && o.FStatus == 2)
                    .Select(a => (double?)a.FTotal)
                    .Average() ?? 0, 2);

            // 修正：正確比較上個月的平均訂單金額
            double AvgOrderTotalsLastMonth = Math.Round(
                _VegetableContext.TOrders
                    .Where(o => o.FOrderAt.Month == DateTime.Now.AddMonths(-1).Month && o.FOrderAt.Year == DateTime.Now.AddMonths(-1).Year && o.FStatus == 2)
                    .Select(a => (double?)a.FTotal)
                    .Average() ?? 0, 2);

            // 修正：確保計算的是「昨天」的訂單平均金額
            double AvgOrderTotalsLastDay = Math.Round(
                _VegetableContext.TOrders
                    .Where(o => o.FOrderAt.Date == DateTime.Now.Date.AddDays(-1) && o.FStatus == 2)
                    .Select(a => (double?)a.FTotal)
                    .Average() ?? 0, 2);

            // 計算成長率並存入 ViewBag
            // 檢查去年數據
            if (AvgOrderTotalsLastYear == AvgOrderTotalsYear)
            {
                ViewBag.AvgOrderTotalsPercentageYear = 0; // 當年和去年數據都為零，設為零
            }
            else
            {
                ViewBag.AvgOrderTotalsPercentageYear = AvgOrderTotalsLastYear != 0
                    ? Math.Round(((decimal)(AvgOrderTotalsYear / AvgOrderTotalsLastYear - 1) * 100), 1)
                    : 100;
            }

            // 檢查上月數據
            if (AvgOrderTotalsLastMonth == AvgOrderTotalsMonth)
            {
                ViewBag.AvgOrderTotalsPercentageMonth = 0; // 當月和上月數據都為零，設為零
            }
            else
            {
                ViewBag.AvgOrderTotalsPercentageMonth = AvgOrderTotalsLastMonth != 0
                    ? Math.Round((decimal)((AvgOrderTotalsMonth / AvgOrderTotalsLastMonth - 1) * 100), 1)
                    : 100;
            }

            // 檢查昨日數據
            if (AvgOrderTotalsLastDay == AvgOrderTotalsDay)
            {
                ViewBag.AvgOrderTotalsPercentageDay = 0; // 當日和昨日數據都為零，設為零
            }
            else
            {
                ViewBag.AvgOrderTotalsPercentageDay = AvgOrderTotalsLastDay != 0
                    ? Math.Round(((decimal)(AvgOrderTotalsDay / AvgOrderTotalsLastDay - 1) * 100), 1)
                    : 100;
            }


            var members = _VegetableContext.TPeople.Where(p => p.FPermission == 0).ToList();
            var startDate = DateTime.Now.Date.AddDays(-29);
            // 確保範圍涵蓋完整30天
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
            var startMonth = DateTime.Now.Date.AddMonths(-11); // 往前推 11 個月，加上當月總共 12 個月
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
                Total = g.Sum(x => x.Total),
                TotalSum = g.Sum(x => x.TotalSum)
            })
            .OrderByDescending(x => x.Total)
            .Take(5)
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
                Total = g.Sum(x => x.Total),
                TotalSum = g.Sum(x => x.TotalSum)
            }).OrderByDescending(x => x.Total)
            .Take(5)
            .ToList();
            var BestSellingProductAll = _VegetableContext.TOrderLists.Join(_VegetableContext.TProducts, ol => ol.FProductId, p => p.FId, (ol, p) => new
            { ol, p }).Join(_VegetableContext.TOrders, b => b.ol.FOrderId, o => o.FId, (b, o) => new
            {
                Total = b.ol.FCount,
                TotalSum = b.p.FPrice * b.ol.FCount,
                b.p.FName,
                o.FStatus
            }).Where(x => x.FStatus == 2).GroupBy(x => x.FName).Select(g => new
            {
                ProductName = g.Key,
                Total = g.Sum(x => x.Total),
                TotalSum = g.Sum(x => x.TotalSum)
            }).OrderByDescending(x => x.Total)
            .Take(5)
            .ToList();

            var MostPopularProduct = _VegetableContext.TFavorites
    .GroupBy(f => f.FProductId) // 先根據產品 ID 分組
    .Select(g => new
    {
        ProductId = g.Key,
        Likes = g.Count() // 計算該產品的喜愛數量
    })
    .Join(_VegetableContext.TProducts, f => f.ProductId, p => p.FId,
        (f, p) => new
        {
            ProductName = p.FName,
            ProductId = p.FId,
            Likes = f.Likes
        })
    .GroupJoin(_VegetableContext.TComments, j => j.ProductId, c => c.FProductId,
        (j, comments) => new
        {
            ProductName = j.ProductName,
            ProductId = j.ProductId,
            Likes = j.Likes,
            Star = comments.Any() ? comments.Average(c => c.FStar) : 0 // 計算平均星級
        })
    .OrderByDescending(y => y.Likes) // 根據喜愛數量排序
    .Take(5)
    .ToList();



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
            List<int> SellingClassAll = _VegetableContext.TOrderLists
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
                    State = order.FStatus
                })
                .Where(t => t.State == 2)
                .GroupBy(x => x.Classification)
                .Select(g => g.Sum(x => x.Count))
                .ToList();

            var UnDoneOrder = _VegetableContext.TOrders.Where(x => x.FStatus == 1 && x.FOrderAt < DateTime.Now.AddDays(-3)).Select(x => x.FId).ToList();
            var viewmodel = new CERPIndexViewModel
            {
                AllMembersLabels = TotalMembersAll.Keys.ToList(),
                TotalMembers = _VegetableContext.TPeople.Count(p => p.FPermission == 0),
                TotalMembersAll = TotalMembersAll.Values.ToList(),
                TotalMembersYear = TotalMembersYear.Values.ToList(),
                TotalMembersMonth = TotalMembersMonth.Values.ToList(),
                AvgOrderTotalsYear = AvgOrderTotalsYear,
                AvgOrderTotalsMonth = AvgOrderTotalsMonth,
                AvgOrderTotalsDay = AvgOrderTotalsDay,
                TotalOrdersYear = TotalOrdersYear,
                TotalOrdersMonth = TotalOrdersMonth,
                TotalOrdersDay = TotalOrdersDay,
                BestSellingProductYear = BestSellingProductYear,
                BestSellingProductMonth = BestSellingProductMonth,
                BestSellingProductAll = BestSellingProductAll,

                MostPopularProduct = MostPopularProduct,

                SellingClassYear = SellingClassYear,
                SellingClassMonth = SellingClassMonth,
                SellingClassAll = SellingClassAll,
                UnDoneOrder = UnDoneOrder
            };
            return View(viewmodel);
        }



        public IActionResult CardSetting()
        {
            return View();
        }


        public IActionResult CarouselSetting()
        {
            return View();
        }
    }
}