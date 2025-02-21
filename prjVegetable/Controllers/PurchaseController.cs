using Microsoft.AspNetCore.Mvc;
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
            // 取得登入使用者 ID
            var checkout = int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId);

            if (!checkout)
            {
                return RedirectToAction("Index", "Home"); // 未登入則導回首頁
            }

            DbVegetableContext db = new DbVegetableContext();
            string? keyword = vm.txtKeyword;

            // 先查詢所有的供應商名稱，轉為 Dictionary 方便查找
            var providers = db.TProviders.ToDictionary(p => p.FId, p => p.FName);

            IEnumerable<TPurchase> datas;

            // 根據關鍵字篩選採購資料
            if (string.IsNullOrEmpty(keyword))
            {
                datas = db.TPurchases.ToList();
            }
            else
            {
                datas = db.TPurchases.Where(p =>
                    p.FId.ToString().Contains(keyword) ||
                    p.FBuyDate.ToString().Contains(keyword) ||
                    p.FProviderId.ToString().Contains(keyword) ||
                    p.FInvoiceForm.ToString().Contains(keyword) ||
                    p.FTotal.ToString().Contains(keyword) ||
                    p.FTax.ToString().Contains(keyword) ||
                    p.FPreTax.ToString().Contains(keyword) ||
                    p.FNote.ToString().Contains(keyword) ||
                    p.FEditor.ToString().Contains(keyword)
                ).ToList();
            }

            // 轉換成 CPurchaseWrap，並加入供應商名稱
            List<CPurchaseWrap> list = datas.Select(t => new CPurchaseWrap()
            {
                Purchase = t,
                FProviderName = providers.ContainsKey(t.FProviderId) ? providers[t.FProviderId] : "未知供應商"
            }).ToList();

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

            // 取得 TProduct 的所有 FName，并传递给前端
            ViewBag.ProductList = _dbContext.TProviders
                                         .Select(p => new { p.FId, p.FName })
                                         .ToList();

            //insert id into FEditor
            var viewModel = new CPurchaseWrap
            {
                Purchase = new TPurchase
                {
                    FEditor = userId,
                    FBuyDate=new DateTime(2025, 3, 7)
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
            // 取得 TProduct 的所有 FName，并传递给前端
            ViewBag.ProductList = _dbContext.TProviders
                                         .Select(p => new { p.FId, p.FName })
                                         .ToList();

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
                    x.FNote = p.FNote ?? "";

                    db.SaveChanges();
                }
            }

            return RedirectToAction("List"); // 修改完成後，跳轉回列表頁
        }


    }
}
