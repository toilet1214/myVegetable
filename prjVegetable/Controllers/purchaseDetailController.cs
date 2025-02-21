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
            if (!int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
            {
                return RedirectToAction("Index", "Home");
            }

            DbVegetableContext db = new DbVegetableContext();
            string keyword = vm.txtKeyword;

            // 取得 TPurchaseDetail 資料
            var query = db.TPurchaseDetails.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.FPurchaseId.ToString().Contains(keyword) || p.FProductId.ToString().Contains(keyword));
            }

            var list = query
                .Join(db.TProducts,
                      pd => pd.FProductId,
                      p => p.FId,
                      (pd, p) => new CPurchaseDetailWrap
                      {
                          PurchaseDetail = pd,
                          FProductIdVision = p.FName // 透過 Join 取得商品名稱
                      })
                .ToList();

            return View(list);
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

            // 取得 TProduct 的所有 FName，并传递给前端
            ViewBag.ProductList = _dbContext.TProducts
                                         .Select(p => new { p.FId, p.FName })
                                         .ToList();
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

            // 取得 TProduct 的所有 FName，并传递给前端
            ViewBag.ProductList = _dbContext.TProducts
                                         .Select(p => new { p.FId, p.FName })
                                         .ToList();

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

            // 建立資料庫上下文
            using (DbVegetableContext db = new DbVegetableContext())
            {
                // 查找對應的 TPurchaseDetail
                TPurchaseDetail x = db.TPurchaseDetails.FirstOrDefault(c => c.FId == p.FId);
                if (x != null)
                {
                    x.FPurchaseId = p.FPurchaseId;
                    x.FProductId = p.FProductId;
                    x.FCount = p.FCount;
                    x.FPrice = p.FPrice;
                    x.FSum = p.FSum;
                }

                // 查找對應的 TPurchase（通過 FId 關聯）
                TPurchase y = db.TPurchases.FirstOrDefault(c => c.FId == p.FId);
                if (y != null)
                {
                    y.FEditor = userId; // 更新 FEditor
                }

                // 一次性保存所有更改
                db.SaveChanges();
            }

            return RedirectToAction("List");
        }

    }
}
