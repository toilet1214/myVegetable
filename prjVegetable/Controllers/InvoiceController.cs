using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class InvoiceController : Controller
    {
        //----------(會員session)------------------
        private readonly ILogger<InvoiceController> _logger;
        private readonly DbVegetableContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        public InvoiceController(ILogger<InvoiceController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = environment;
        }

        //----------List------------------------
        public IActionResult List(CKeywordViewModel vm)
        {
            // 先session
            var checkout = int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId);

            // 先驗證身分
            if (!checkout)
            {
                return RedirectToAction("Index", "Home"); // 若未登入，跳轉至登入頁面
            }

            DbVegetableContext db = new DbVegetableContext();
            string? keyword = vm.txtKeyword;

            // 先查詢所有的供應商資料，轉為 Dictionary 方便查找
            var providers = db.TProviders.ToDictionary(p => p.FId, p => new { p.FName, p.FUbn });

            // 查詢發票資料
            IEnumerable<TInvoice> datas = string.IsNullOrEmpty(keyword) ?
                db.TInvoices :
                db.TInvoices.Where(p =>
                    p.FCustomerId.ToString().Contains(keyword) ||
                    p.FCustomerUbn.Contains(keyword) ||
                    p.FProviderId.ToString().Contains(keyword) ||
                    p.FProviderUbn.Contains(keyword) ||
                    p.FForm.Contains(keyword) ||
                    p.FInOut.ToString().Contains(keyword) ||
                    p.FStatus.ToString().Contains(keyword) ||
                    p.FDate.ToString().Contains(keyword) ||
                    p.FNumber.Contains(keyword) ||
                    p.FEditor.ToString().Contains(keyword) ||
                    p.FTotal.ToString().Contains(keyword) ||
                    p.FId.ToString().Contains(keyword));

            // 查詢發票明細，計算發票總額 (FSum 總和)
            var invoiceTotals = db.TInvoiceDetails
                .GroupBy(d => d.FNumber)  // 依發票號碼分組
                .ToDictionary(g => g.Key, g => g.Sum(d => d.FSum)); // 計算該發票的小計總額

            // 轉換成 CInvoiceWrap，包含供應商名稱與供應商統編，並填入計算後的總額
            List<CInvoiceWrap> list = datas.Select(t => new CInvoiceWrap()
            {
                TInvoice = t,
                FProviderName = providers.ContainsKey(t.FProviderId) ? providers[t.FProviderId].FName : "未知供應商",
                FProviderUbn = providers.ContainsKey(t.FProviderId) ? providers[t.FProviderId].FUbn : "未知統編",
                FTotals = invoiceTotals.ContainsKey(t.FNumber) ? invoiceTotals[t.FNumber] : 0 // 填入計算後的總額
            }).ToList();

            return View(list);
        }



        //----------Create------------------------
        public IActionResult Create()
        {
            //先session
            var checkout = int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId);

            // 先驗證身分
            if (!checkout == true)
            {
                return RedirectToAction("List"); // 若未登入，跳轉至登入頁面
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("List"); // 若驗證失敗，回到List
            }

            // 取得 TProduct 的所有 FName，并传递给前端
            ViewBag.ProductList = _dbContext.TProviders
                .Select(p => new { p.FId, p.FName, p.FUbn })
                .ToList();


            //insert id into FEditor
            var viewModel = new CInvoiceWrap
            {
                TInvoice = new TInvoice
                {
                    FEditor = userId,
                    FDate = new DateTime(2025, 3, 7)
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(TInvoice p)
        {
            DbVegetableContext db = new DbVegetableContext();
            db.TInvoices.Add(p);
            db.SaveChanges(); //回傳至資料庫
            return RedirectToAction("List");
        }

        //----------"delete" 改為"作廢功能"----------------------
        public ActionResult Delete(int? id) //int? => 允許有null
        {
            // 先驗證身分
            if (!int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
            {
                return RedirectToAction("List"); // 若未登入，跳轉至登入頁面
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("List"); // 若驗證失敗，回到List
            }

            //
            if (id != null)
            {
                //open database 
                DbVegetableContext db = new DbVegetableContext();

                //判斷輸入的id 是否存在資料庫裡面: 點選作廢
                TInvoice x = db.TInvoices.FirstOrDefault(c => c.FId == id); //Linq 語法: 判斷
                if (x != null)
                {
                    x.FEditor = userId;
                    x.FStatus = 1;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        //----------edit------------------------
        public IActionResult Edit(int? id)

        {
            //如果id為null 則回list
            if (id == null)
                return RedirectToAction("List");

            //建立資料庫
            DbVegetableContext db = new DbVegetableContext();

            // 根據傳入的 id 查找對應的客戶資料
            TInvoice x = db.TInvoices.FirstOrDefault(c => c.FId == id);
            CInvoiceWrap c = new CInvoiceWrap() { TInvoice=x};
            // 如果找不到該客戶，重導向到 List 動作
            if (x == null)
                return RedirectToAction("List");

            // 取得 TProduct 的所有 FName，并传递给前端
            ViewBag.ProductList = _dbContext.TProviders
                .Select(p => new { p.FId, p.FName, p.FUbn })
                .ToList();

            // 將找到的客戶資料傳遞到 View
            return View(c);
        }
        [HttpPost]
        public IActionResult Edit(TInvoice p)
        {
            // 先驗證身分
            if (!int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
            {
                return RedirectToAction("List"); // 若未登入，跳轉至登入頁面
            }

            //若驗證失敗，回到編輯頁面
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id = p.FId });
            }

            //建立資料庫
            DbVegetableContext db = new DbVegetableContext();

            //搜尋id : "p.Fid" 為資料庫裡的id。 "c.Fid"為輸入的id
            TInvoice x = db.TInvoices.FirstOrDefault(c => c.FId == p.FId);
            
            if (x != null)
            {
               
                x.FNumber = p.FNumber;
                x.FDate = p.FDate;
                x.FForm = p.FForm;
                x.FCustomerId = p.FCustomerId;
                x.FCustomerUbn = p.FCustomerUbn;
                x.FProviderId = p.FProviderId;
                x.FProviderUbn = p.FProviderUbn;
                x.FInOut = p.FInOut;
                x.FStatus = p.FStatus;
                x.FTotal = p.FTotal;
                x.FEditor = userId; // 記錄目前登入者的 ID

                db.SaveChanges();

            }
            return RedirectToAction("List");
        }
    }
}
