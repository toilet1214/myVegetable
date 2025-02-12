using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Linq;

namespace prjVegetable.Controllers
{
    public class InvoiceDetailController : Controller
    {
        //----------(會員session)------------------
        private readonly ILogger<InvoiceDetailController> _logger;
        private readonly DbVegetableContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        public InvoiceDetailController(ILogger<InvoiceDetailController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = environment;
        }

        //----------List------------------------
        public IActionResult List(CKeywordViewModel vm)
        {
            // 先驗證登入狀態
            if (!int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
            {
                return RedirectToAction("Index", "Home"); // 若未登入，跳轉至首頁
            }

            using (DbVegetableContext db = new DbVegetableContext())
            {
                string? keyword = vm.txtKeyword;
                IEnumerable<TInvoiceDetail> datas = null;

                // 找出當前使用者編輯過的發票號碼 (FNumber)
                var editedInvoiceNumbers = db.TInvoices
                                             .Where(i => i.FEditor == userId)
                                             .Select(i => i.FNumber)
                                             .ToList();

                if (string.IsNullOrEmpty(keyword))
                {
                    // 只顯示該使用者編輯過的發票明細
                    datas = db.TInvoiceDetails
                              .Where(d => editedInvoiceNumbers.Contains(d.FNumber));
                }
                else
                {
                    // 附加關鍵字篩選條件
                    datas = db.TInvoiceDetails
                              .Where(d => editedInvoiceNumbers.Contains(d.FNumber) &&
                                         (d.FId.ToString().Contains(keyword) ||
                                          d.FNumber.Contains(keyword) ||
                                          d.FProductName.Contains(keyword)));
                }

                // 包裝成 ViewModel
                List<CInvoiceDetailWrap> list = datas.Select(d => new CInvoiceDetailWrap() { InvoiceDetail = d }).ToList();
                return View(list);
            }
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

            return View();
        }

        [HttpPost]
        public IActionResult Create(TInvoiceDetail p)
        {
            DbVegetableContext db = new DbVegetableContext();
            db.TInvoiceDetails.Add(p);
            db.SaveChanges(); //回傳至資料庫
            return RedirectToAction("List");
        }
        //----------delete----------------------
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

                //判斷輸入的id 是否存在資料庫裡面
                TInvoiceDetail x = db.TInvoiceDetails.FirstOrDefault(c => c.FId == id); //Linq 語法: 判斷
                if (x != null)
                {
                    db.TInvoiceDetails.Remove(x);
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
            TInvoiceDetail x = db.TInvoiceDetails.FirstOrDefault(c => c.FId == id);
            CInvoiceDetailWrap w = new CInvoiceDetailWrap() { InvoiceDetail=x};
            // 如果找不到該客戶，重導向到 List 動作
            if (x == null)
                return RedirectToAction("List");

            // 將找到的客戶資料傳遞到 View
            return View(w);
        }
        [HttpPost]
        public IActionResult Edit(TInvoiceDetail p)
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
            TInvoiceDetail x = db.TInvoiceDetails.FirstOrDefault(c => c.FId == p.FId);

            if (x != null)
            {
                x.FNumber = p.FNumber;
                x.FProductName = p.FProductName;
                x.FCount = p.FCount;
                x.FPrice = p.FPrice;
                x.FSum = p.FSum;
                db.SaveChanges();

            }
            return RedirectToAction("List");
        }

    }
}
