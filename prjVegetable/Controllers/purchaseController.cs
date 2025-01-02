using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
	public class PurchaseController : Controller
	{
		//----------List------------------------
		public IActionResult List(CKeywordViewModel vm)
		{
            //額外使用"CKeywordViewModel"內的引數，區分request/required 的回傳值
            DbVegetableContext db = new DbVegetableContext();
			string? keyword = vm.txtKeyword;


			//view()呈現
			IEnumerable<TPurchase> datas = null;

			//查詢使用者輸入關鍵字，進入資料庫尋找:(1)關鍵字是否空值 (2)
			if (string.IsNullOrEmpty(keyword))
				datas = from t in db.TPurchases
						select t;
			else
				datas = db.TPurchases.Where(
				 p => p.FSupplierId.Contains(keyword)
				|| p.FSupplierName.Contains(keyword)
				|| p.FBuyer.Contains(keyword)
				|| p.FId.ToString().Contains(keyword));

			//原TPurchase 擴展為CTPurchaseWrap(綠框): CTPurchaseWrap 為TPurchase的擴展。目的為，若有資料變動的時候，可以不造成程式碼更動太大。
			List<CPurchaseWrap> list = new List<CPurchaseWrap>();
			foreach (var t in datas)
				list.Add(new CPurchaseWrap() { Purchase = t });
			return View(list);
		}

		//----------create------------------------
		public IActionResult create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult create(TPurchase p)
		{
			DbVegetableContext db = new DbVegetableContext();
			db.TPurchases.Add(p);
			db.SaveChanges(); //回傳至資料庫
			return RedirectToAction("List");
		}
		//----------delete----------------------
		public ActionResult Delete(int? id) //int? => 允許有null

		{
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
			//建立資料庫
			DbVegetableContext db = new DbVegetableContext();

			//搜尋id : "p.Fid" 為資料庫裡的id。 "c.Fid"為輸入的id
			TPurchase x = db.TPurchases.FirstOrDefault(c => c.FId == p.FId);

			if (x != null)
			{
				x.FBuyDate = p.FBuyDate;
				x.FSupplierId = p.FSupplierId;
				x.FSupplierName = p.FSupplierName;
				x.FBuyer = p.FBuyer;
				x.FExpirationDate = p.FExpirationDate;
				x.FIsTax = p.FIsTax;
				x.FInvoiceForm = p.FInvoiceForm;
				x.FPayment = p.FPayment;
				x.FCreater = p.FCreater;
				x.FEditor = p.FEditor;
				x.FPreTax = p.FPreTax;
				x.FTax = p.FTax;
				x.FTotal = p.FTotal;
				x.FNote = p.FNote;
				db.SaveChanges();

			}
			return RedirectToAction("List");
		}
	}
}
