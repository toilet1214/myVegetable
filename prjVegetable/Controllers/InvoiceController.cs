using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using team_20241228.Models;
using team_20241228.ViewModels;

namespace team_20241228.Controllers
{
    public class InvoiceController : Controller
    {
        //----------List------------------------
        public IActionResult List(CkeywordViewModels vm)
        {
            //額外使用"CKeywordViewModels"內的引數，區分request/required 的回傳值
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
                || p.FSupplierId.ToString().Contains(keyword)
                || p.FSupplierUbn.Contains(keyword)
                || p.FDate.ToString().Contains(keyword)
                || p.FNumber.Contains(keyword)
                || p.FId.ToString().Contains(keyword));

            //原TPurchase 擴展為CTPurchaseWrap(綠框): CTPurchaseWrap 為TPurchase的擴展。目的為，若有資料變動的時候，可以不造成程式碼更動太大。
            List<CTInvoiceWrap> list = new List<CTInvoiceWrap>();
            foreach (var t in datas)
                list.Add(new CTInvoiceWrap() { TInvoice = t });
            return View(list);
        }

        //----------create------------------------
        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult create(TInvoice p)
        {
            DbVegetableContext db = new DbVegetableContext();
            db.TInvoices.Add(p);
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
            CTInvoiceWrap c = new CTInvoiceWrap() { TInvoice=x};
            // 如果找不到該客戶，重導向到 List 動作
            if (x == null)
                return RedirectToAction("List");

            // 將找到的客戶資料傳遞到 View
            return View(c);
        }
        [HttpPost]
        public IActionResult Edit(TInvoice p)

        {
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
                x.FSupplierId = p.FSupplierId;
                x.FSupplierUbn = p.FSupplierUbn;
                x.FInOut = p.FInOut;
                x.FStatus = p.FStatus;
                x.FTotal = p.FTotal;
                x.FEditor = p.FEditor;

                db.SaveChanges();

            }
            return RedirectToAction("List");
        }
    }
}
