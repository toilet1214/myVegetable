using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;

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


        public IActionResult Order()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            DbVegetableContext db = new DbVegetableContext();
            IEnumerable<TOrder> datas = null;
            
            datas = db.TOrders.Where(p => p.FBuyerId == UserId);
            List<COrderWrap> list = new List<COrderWrap>();
            foreach (var t in datas)
                list.Add(new COrderWrap() { order = t });
            return View(list);
        }

        public IActionResult OrderDetail(int? id)
        {
            if (id == null)
                return RedirectToAction("Order");
            DbVegetableContext db = new DbVegetableContext();
            IEnumerable<TOrderList>datas = null;
            datas = db.TOrderLists.Where(p => p.FOrderId == id);
            List<COrderListWrap> list = new List<COrderListWrap>();
            foreach (var t in datas)
                list.Add(new COrderListWrap() { orderList = t });
            return View(list);
        }

    }
}
