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
            //先session
            var checkout = int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId);

            // 先驗證身分
            if (!checkout == true)
            {
                return RedirectToAction("Index", "Home"); // 若未登入，跳轉至登入頁面
            }

            //if (ModelState.IsValid)
            //{
            //    return RedirectToAction("List", "purchaseDetail"); // 若驗證成功，回到Views():Purchase/List
            //}

            //額外使用"CKeywordViewModels"內的引數，區分request/required 的回傳值
            DbVegetableContext db = new DbVegetableContext();
            string keyword = vm.txtKeyword;

            //view()呈現
            IEnumerable<TPurchaseDetail> datas = null;

            //查詢使用者輸入關鍵字，進入資料庫尋找:(1)關鍵字是否空值 (2)
            if (string.IsNullOrEmpty(keyword))
                datas = from t in db.TPurchaseDetails
                        select t;
            else
                datas = db.TPurchaseDetails.Where(
                 p => 
                 p.FPurchaseId.ToString().Contains(keyword)
                 || p.FProductId.ToString().Contains(keyword)
                 || p.FCount.ToString().Contains(keyword)
                 || p.FCount.ToString().Contains(keyword)
                 || p.FSum.ToString().Contains(keyword));

            //原TPurchase 擴展為CTPurchaseWrap(綠框): CTPurchaseWrap 為TPurchase的擴展。目的為，若有資料變動的時候，可以不造成程式碼更動太大。
            List<CPurchaseDetailWrap> list = new List<CPurchaseDetailWrap>();
            foreach (var t in datas)
                list.Add(new CPurchaseDetailWrap() { PurchaseDetail = t });
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
