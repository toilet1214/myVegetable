using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using System.Collections.Generic;

namespace prjVegetable.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            DbVegetableContext db = new DbVegetableContext();
            TPerson x = db.TPeople.FirstOrDefault(c => c.FId == UserId);
            return View(new CCustomerWrap() { person = x });
        }
        [HttpPost]
        public IActionResult Index(CCustomerWrap p)
        {
            DbVegetableContext db = new DbVegetableContext();
            TPerson x = db.TPeople.FirstOrDefault(c => c.FId == p.FId);
            if (x != null)
            {
                x.FName = p.FName;
                x.FBirth = p.FBirth;
                x.FEmail = p.FEmail;
                x.FPhone = p.FPhone;
                x.FTel = p.FTel;
                x.FAddress = p.FAddress;
                x.FPassword = p.FPassword;
                x.FUbn = p.FUbn;
                x.FGender = p.FGender;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Report()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Report(TReport P)
        {
            DbVegetableContext db = new DbVegetableContext();
            db.TReports.Add(P);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Order()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            DbVegetableContext db = new DbVegetableContext();
            IEnumerable<TOrder> datas = null;

            datas = db.TOrders.Where(p => p.FPersonId == UserId);
            List<COrderWrap> list = new List<COrderWrap>();
            foreach (var t in datas)
                list.Add(new COrderWrap() { order = t });
            return View(list);
        }

        public IActionResult OrderDetail(int? id) //orderid
        {
            if (id == null)
                return RedirectToAction("Order");

            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId);

            DbVegetableContext db = new DbVegetableContext();

            var orderList = db.TOrderLists.Where(ol => ol.FOrderId == id).ToList();
            var order = db.TOrders.FirstOrDefault(p => p.FId == id && p.FPersonId == userId);

            //IEnumerable<TOrderList> datas = null;
            //datas = db.TOrderLists.Where(p => p.FOrderId == id);

            List<COrderListWrap> list = new List<COrderListWrap>();
            foreach (var t in orderList)
            {
                var hasComment = db.TComments
                .Where(c => c.FOrderListId == t.FId && c.FProductId == t.FProductId && c.FPersonId == userId)
                .Any();
                list.Add(new COrderListWrap() 
                {
                    orderList = t, 
                    OrderStatus = order.FStatus,
                    HasComment = hasComment,
                });
            }
                
            return View(list);
        }
        

        public IActionResult AddComment(int? id) //orderlistid
        {
            
            if (id == null)
            {
                return RedirectToAction("OrderDetail");
            }

            DbVegetableContext db = new DbVegetableContext();
            
            var orderList = db.TOrderLists.FirstOrDefault(ol => ol.FId == id);
            var order = db.TOrders.FirstOrDefault(o => o.FId == orderList.FOrderId);
            var product = db.TProducts.FirstOrDefault(p => p.FId == orderList.FProductId);

            var CCommentWrap = new CCommentWrap
            {
                FPersonId = order.FPersonId,
                FProductId = orderList.FProductId,
                FOrderListId = orderList.FId,
                FOrderId = order.FId,
                FProductName = product?.FName ?? "未知商品",

            };
            return View(CCommentWrap);
        }

        //加入評論
        [HttpPost]
        public IActionResult AddComment(TComment c)
        {
            DbVegetableContext db = new DbVegetableContext();
            c.FStar = c.FStar == 0 ? 5 : c.FStar;

            db.TComments.Add(c);
            db.SaveChanges();

            
            return RedirectToAction("Order");
        }

        public IActionResult CommentIndex()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            DbVegetableContext db = new DbVegetableContext();
            //var product = db.TProducts.FirstOrDefault(p=>p.FId ==)
            //var person = db.TPeople.FirstOrDefault(pp=>pp.FId ==)
            var datas = db.TComments
                        .Where(c => c.FPersonId == UserId)
                        .Join(
                            db.TOrderLists,
                            comment => comment.FOrderListId,
                            orderList => orderList.FId,
                            (comment, orderList) => new { comment, orderList }
                        )
                        .Select(x => new CCommentWrap
                        {
                            FId = x.comment.FId,
                            FComment = x.comment.FComment,
                            FStar = x.comment.FStar,
                            FPersonName = db.TPeople.Where(pp => pp.FId == x.comment.FPersonId).FirstOrDefault().FName,
                            FProductName = db.TProducts.Where(p => p.FId == x.comment.FProductId).FirstOrDefault().FName,
                            FOrderId = x.orderList.FOrderId,
                            FCreatedAt = x.comment.FCreatedAt

                        })
                         .OrderByDescending(x => x.FCreatedAt)
                        .ToList();

            return View(datas);
        }

    }
}
