﻿using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class InvoiceDetailController : Controller
    {
        //----------List------------------------
        public IActionResult List(CKeywordViewModel vm)
        {
            //額外使用"CKeywordViewModel"內的引數，區分request/required 的回傳值
            DbVegetableContext db = new DbVegetableContext();
            string? keyword = vm.txtKeyword;


            //view()呈現
            IEnumerable<TInvoiceDetail> datas = null;

            //查詢使用者輸入關鍵字，進入資料庫尋找:(1)關鍵字是否空值 (2)
            if (string.IsNullOrEmpty(keyword))
                datas = from t in db.TInvoiceDetails
                        select t;
            else
                datas = db.TInvoiceDetails.Where(
                 p => p.FId.ToString().Contains(keyword)
                || p.FNumber.Contains(keyword)
                || p.FProductName.Contains(keyword));

            //原TPurchase 擴展為CTPurchaseWrap(綠框): CTPurchaseWrap 為TPurchase的擴展。目的為，若有資料變動的時候，可以不造成程式碼更動太大。
            List<CInvoiceDetailWrap> list = new List<CInvoiceDetailWrap>();
            foreach (var t in datas)
                list.Add(new CInvoiceDetailWrap() { InvoiceDetail = t });
            return View(list);
        }

        //----------Create------------------------
        public IActionResult Create()
        {
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
            //建立資料庫
            DbVegetableContext db = new DbVegetableContext();

            //搜尋id : "p.Fid" 為資料庫裡的id。 "c.Fid"為輸入的id
            TInvoiceDetail x = db.TInvoiceDetails.FirstOrDefault(c => c.FId == p.FId);

            if (x != null)
            {
                x.FNumber = p.FNumber;
                x.FProductName = p.FProductName;
                x.FConut = p.FConut;
                x.FPrice = p.FPrice;
                x.FSum = p.FSum;
                db.SaveChanges();

            }
            return RedirectToAction("List");
        }

    }
}
