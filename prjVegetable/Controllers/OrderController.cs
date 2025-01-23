using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult List(CKeywordViewModel vm)
        {
            string keyword = vm.txtKeyword;
            DbVegetableContext db = new DbVegetableContext();
            IEnumerable<TOrder> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = from t in db.TOrders select t;
            else { 
                
            }
                
            List<COrderWrap> list = new List<COrderWrap>();
            foreach (var t in datas)
                list.Add(new COrderWrap() { order = t });
            return View(list);
        }

        public IActionResult OrderList(int? id)
        {
            if (id == null)
                return RedirectToAction("Order");
            DbVegetableContext db = new DbVegetableContext();
            IEnumerable<TOrderList> datas = null;
            datas = db.TOrderLists.Where(p => p.FOrderId == id);
            List<COrderListWrap> list = new List<COrderListWrap>();
            foreach (var t in datas)
                list.Add(new COrderListWrap() { orderList = t });
            return View(list);
        }

        
    }
}
