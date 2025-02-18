using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class AjaxBusinessOperationController : Controller
    {

        //database
        private readonly DbVegetableContext _context;
        public AjaxBusinessOperationController(DbVegetableContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //ViewBag.Invoices = _context.TInvoices.ToList();
            //ViewBag.InvoiceDetails = _context.TInvoiceDetails.ToList();
            return View();
        }

        //datasets


        //API-purchase
        public JsonResult GetPurchaseData(DateTime startDate, DateTime endDate)
        {
            var purchases = _context.TPurchases
                .Where(p => p.FBuyDate >= startDate && p.FBuyDate <= endDate)
                .ToList();

            var totalAmount = purchases.Sum(p => p.FTotal);

            // 支付方式總額
            var paymentTotals = purchases
                .GroupBy(p => p.FPayment)
                .Select(g => new
                {
                    label = g.Key.ToString(), // 轉換支付方式為字串
                    y = g.Sum(p => p.FTotal) // 計算總額
                })
                .ToList();

            // 支付方式分類的數量
            var paymentMethods = purchases
                .GroupBy(p => p.FPayment)
                .Select(g => new
                {
                    label = g.Key.ToString(), // 轉換支付方式為字串
                    y = g.Count() // 計算筆數
                })
                .ToList();

            // **新增月份金額趨勢**
            var monthlyTrends = purchases
                .GroupBy(p => new { p.FBuyDate.Year, p.FBuyDate.Month }) // 按年 + 月分組
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month) // 按時間排序
                .Select(g => new
                {
                    x = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM-dd"), // 設定為該月第一天
                    y = g.Sum(p => p.FTotal) // 該月總金額
                })
                .ToList();

            return new JsonResult(new
            {
                total = totalAmount,
                paymentTotals = paymentTotals,
                paymentMethods = paymentMethods,
                monthlyTrends = monthlyTrends // 回傳金額趨勢圖數據
            });
        }

        //API-invoice
        [HttpGet]
        public JsonResult GetInvoiceData(DateTime startDate, DateTime endDate)
        {
            var invoices = _context.TInvoices
                .Where(i => i.FDate >= startDate && i.FDate <= endDate)
                .ToList();

            int totalAmount = invoices.Sum(i => i.FTotal);
            int totalCount = invoices.Count;

            var invoiceTypes = invoices
                .GroupBy(i => i.FInOut)
                .Select(g => new { label = g.Key == 0 ? "進項" : "銷項", y = g.Count() })
                .ToList();

            var statusCounts = invoices
                .GroupBy(i => i.FStatus)
                .Select(g => new { label = g.Key == 0 ? "一般" : "作廢", y = g.Count() })
                .ToList();

            // 获取时间范围内的所有月份
            List<object> timeTrends = new List<object>();
            DateTime current = new DateTime(startDate.Year, startDate.Month, 1);
            DateTime end = new DateTime(endDate.Year, endDate.Month, 1);

            var monthlyData = invoices
                .GroupBy(i => new { Year = i.FDate.Year, Month = i.FDate.Month })
                .ToDictionary(g => new DateTime(g.Key.Year, g.Key.Month, 1), g => g.Count());

            while (current <= end)
            {
                timeTrends.Add(new
                {
                    x = current.ToString("yyyy-MM-dd"), // 确保前端可以正确解析
                    y = monthlyData.ContainsKey(current) ? monthlyData[current] : 0
                });
                current = current.AddMonths(1);
            }

            return Json(new
            {
                total = totalAmount,
                count = totalCount,
                invoiceTypes,
                statusCounts,
                timeTrends
            });
        }

        //API-invoice detail+invoice





        [HttpGet]
        public IActionResult GetProductsData(DateTime startDate, DateTime endDate)
        {
            var productQuantityData = _context.TInvoiceDetails
                .Join(_context.TInvoices, d => d.FNumber, i => i.FNumber, (d, i) => new { d, i })
                .Where(di => di.i.FDate >= startDate && di.i.FDate <= endDate)
                .GroupBy(di => di.d.FProductName)
                .Select(g => new { label = g.Key, y = g.Sum(di => di.d.FCount) })
                .ToList();

            var totalAmount = _context.TInvoiceDetails
                .Join(_context.TInvoices, d => d.FNumber, i => i.FNumber, (d, i) => new { d, i })
                .Where(di => di.i.FDate >= startDate && di.i.FDate <= endDate)
                .Sum(di => di.d.FSum);

            var categoryAmountData = _context.TInvoiceDetails
                .Join(_context.TInvoices, d => d.FNumber, i => i.FNumber, (d, i) => new { d, i })
                .Where(di => di.i.FDate >= startDate && di.i.FDate <= endDate)
                .GroupBy(di => di.d.FProductName)
                .Select(g => new { label = g.Key, y = totalAmount > 0 ? (double)g.Sum(di => di.d.FSum) / totalAmount * 100 : 0 })
                .ToList();

            

            return Ok(new { productQuantityData, categoryAmountData });
        }

    }
}
