using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Text.Json;

namespace prjVegetable.Controllers
{
	public class PurchaseController : Controller
	{
        //----------(會員session)------------------
        private readonly ILogger<PurchaseController> _logger;
        private readonly DbVegetableContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        public PurchaseController(ILogger<PurchaseController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
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
                
                return RedirectToAction("Index","Home"); // 若未登入，跳轉至登入頁面
            }

            //if (ModelState.IsValid)
            //{
            //    return RedirectToAction("List","Purchase"); // 若驗證成功，回到Views():Purchase/List
            //}

            //額外使用"CKeywordViewModel"內的引數，區分request/required 的回傳值
            DbVegetableContext db = new DbVegetableContext();
			string? keyword = vm.txtKeyword;


			//view()呈現
			IEnumerable<TPurchase> datas = null;

			//查詢使用者輸入關鍵字，進入資料庫尋找:(1)關鍵字是否空值 或(2)目前資料筆數
			if (string.IsNullOrEmpty(keyword))
				datas = from t in db.TPurchases
						select t;
			else
				datas = db.TPurchases.Where(
				p =>p.FId.ToString().Contains(keyword)
                || p.FBuyDate.ToString().Contains(keyword)
                || p.FProviderId.ToString().Contains(keyword)
                || p.FInvoiceForm.ToString().Contains(keyword)
                || p.FTotal.ToString().Contains(keyword)
                || p.FTax.ToString().Contains(keyword)
                || p.FPreTax.ToString().Contains(keyword)
                || p.FEditor.ToString().Contains(keyword));

			//原TPurchase 擴展為CTPurchaseWrap(綠框): CTPurchaseWrap 為TPurchase的擴展。目的為，若有資料變動的時候，可以不造成程式碼更動太大。
			List<CPurchaseWrap> list = new List<CPurchaseWrap>();
			foreach (var t in datas)
				list.Add(new CPurchaseWrap() { Purchase = t });
			return View(list);
		}

		//----------Create------------------------
		public IActionResult Create()
		{
			//先session
            var checkout=int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId);

            // 先驗證身分
            if ( !checkout == true)
            {
                return RedirectToAction("List"); // 若未登入，跳轉至登入頁面
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("List"); // 若驗證失敗，回到List
            }

            //insert id into FEditor
            var viewModel = new CPurchaseWrap
            {
                Purchase = new TPurchase
                {
                    FEditor = userId
                }             
            };
            return View(viewModel);

        }

        [HttpPost]
		public IActionResult Create(TPurchase p)
		{

            DbVegetableContext db = new DbVegetableContext();
			db.TPurchases.Add(p);
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
				TPurchase x = db.TPurchases.FirstOrDefault(c => c.FId == id); //Linq 語法: 判斷
				if (x != null)
				{
					db.TPurchases.Remove(x);
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
			TPurchase x = db.TPurchases.FirstOrDefault(c => c.FId == id);
			CPurchaseWrap c = new CPurchaseWrap() {Purchase = x };

			// 如果找不到該客戶，重導向到 List 動作
			if (x == null)
				return RedirectToAction("List");

			// 將找到的客戶資料傳遞到 View
			return View(c);
		}

        [HttpPost]
        public IActionResult Edit(TPurchase p)
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

            using (DbVegetableContext db = new DbVegetableContext())
            {
                // 搜尋 id
                TPurchase x = db.TPurchases.FirstOrDefault(c => c.FId == p.FId);

                if (x != null)
                {
                    x.FBuyDate = p.FBuyDate;
                    x.FProviderId = p.FProviderId;
                    x.FInvoiceForm = p.FInvoiceForm;
                    x.FPayment = p.FPayment;
                    x.FEditor = userId; // 記錄目前登入者的 ID
                    x.FPreTax = p.FPreTax;
                    x.FTax = p.FTax;
                    x.FTotal = p.FTotal;
                    x.FNote = p.FNote;

                    db.SaveChanges();
                }
            }

            return RedirectToAction("List"); // 修改完成後，跳轉回列表頁
        }


    }
}
