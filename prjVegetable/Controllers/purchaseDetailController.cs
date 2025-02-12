using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{

    public class purchaseDetailController : Controller
	{
        //----------(會員session)------------------
        private readonly ILogger<purchaseDetailController> _logger;
        private readonly DbVegetableContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        public purchaseDetailController(ILogger<purchaseDetailController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = environment;
        }

        public IActionResult List(CKeywordViewModel vm)
        {
            // 先驗證登入狀態
            if (!int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
            {
                return RedirectToAction("Index", "Home"); // 若未登入，跳轉至首頁
            }

            using (DbVegetableContext db = new DbVegetableContext())
            {
                string keyword = vm.txtKeyword;
                IEnumerable<TPurchaseDetail> datas = null;

                // 先找出該用戶編輯過的採購單
                var editedPurchases = db.TPurchases
                                        .Where(p => p.FEditor == userId)
                                        .Select(p => p.FId)
                                        .ToList();

                if (string.IsNullOrEmpty(keyword))
                {
                    // 只顯示該使用者編輯過的採購單的明細
                    datas = db.TPurchaseDetails
                              .Where(d => editedPurchases.Contains(d.FPurchaseId));
                }
                else
                {
                    // 附加關鍵字篩選條件
                    datas = db.TPurchaseDetails
                              .Where(d => editedPurchases.Contains(d.FPurchaseId) &&
                                         (d.FPurchaseId.ToString().Contains(keyword)
                                         || d.FProductId.ToString().Contains(keyword)));
                }

                // 包裝成 ViewModel
                List<CPurchaseDetailWrap> list = datas.Select(d => new CPurchaseDetailWrap() { PurchaseDetail = d }).ToList();
                return View(list);
            }
        }



        //----------create------------------------
        public IActionResult create()
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
        public IActionResult create(TPurchaseDetail p)
        {
            DbVegetableContext db = new DbVegetableContext();
            db.TPurchaseDetails.Add(p);
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

            if (id != null)
            {
                //open database 
                DbVegetableContext db = new DbVegetableContext();

                //判斷輸入的id 是否存在資料庫裡面
                TPurchaseDetail x = db.TPurchaseDetails.FirstOrDefault(c => c.FId == id); //Linq 語法: 判斷
                if (x != null)
                {
                    db.TPurchaseDetails.Remove(x);
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
            TPurchaseDetail x = db.TPurchaseDetails.FirstOrDefault(c => c.FId == id);
            CPurchaseDetailWrap c = new CPurchaseDetailWrap() { PurchaseDetail=x};

            // 如果找不到該客戶，重導向到 List 動作
            if (x == null)
                return RedirectToAction("List");

            // 將找到的客戶資料傳遞到 View
            return View(c);
        }
        [HttpPost]
        public IActionResult Edit(TPurchaseDetail p)
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
            TPurchaseDetail x = db.TPurchaseDetails.FirstOrDefault(c => c.FId == p.FId);

            if (x != null)
            { 
                x.FPurchaseId = p.FPurchaseId;
                x.FProductId= p.FProductId;
                x.FCount = p.FCount;
                x.FPrice = p.FPrice;
                x.FSum = p.FSum;
                db.SaveChanges();

            }
            return RedirectToAction("List");
        }
    }
}
