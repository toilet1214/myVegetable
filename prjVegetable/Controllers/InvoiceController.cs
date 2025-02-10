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
            //先session
            var checkout = int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId);

            // 先驗證身分
            if (!checkout == true)
            {
                return RedirectToAction("Index", "Home"); // 若未登入，跳轉至登入頁面
            }

            //if (ModelState.IsValid)
            //{
            //    return RedirectToAction("List", "Invoice"); // 若驗證成功，回到Views():Purchase/List
            //}

            //額外使用"CKeywordViewModel"內的引數，區分request/required 的回傳值
            DbVegetableContext db = new DbVegetableContext();
            string? keyword = vm.txtKeyword;


            //view()呈現
            IEnumerable<TInvoice> datas = null;

            //查詢使用者輸入關鍵字，進入資料庫尋找:(1)關鍵字是否空值 (2)
            if (string.IsNullOrEmpty(keyword))
                datas = from t in db.TInvoices
                        select t;
            else
                datas = db.TInvoices.Where(
                 p => p.FCustomerId.ToString().Contains(keyword)
                || p.FCustomerUbn.Contains(keyword)
                || p.FProviderId.ToString().Contains(keyword)
                || p.FProviderUbn.Contains(keyword)
                || p.FForm.Contains(keyword)
                || p.FInOut.ToString().Contains(keyword)
                || p.FStatus.ToString().Contains(keyword)
                || p.FDate.ToString().Contains(keyword)
                || p.FNumber.Contains(keyword)
                || p.FEditor.ToString().Contains(keyword)
                || p.FTotal.ToString().Contains(keyword)
                || p.FId.ToString().Contains(keyword));

            //原TPurchase 擴展為CTPurchaseWrap(綠框): CTPurchaseWrap 為TPurchase的擴展。目的為，若有資料變動的時候，可以不造成程式碼更動太大。
            List<CInvoiceWrap> list = new List<CInvoiceWrap>();
            foreach (var t in datas)
                list.Add(new CInvoiceWrap() { TInvoice = t });
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

            //insert id into FEditor
            var viewModel = new CInvoiceWrap
            {
                TInvoice = new TInvoice
                {
                    FEditor = userId
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
                TInvoice x = db.TInvoices.FirstOrDefault(c => c.FId == id); //Linq 語法: 判斷
                if (x != null)
                {
                    db.TInvoices.Remove(x);
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
