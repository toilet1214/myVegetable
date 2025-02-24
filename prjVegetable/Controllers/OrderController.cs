using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class OrderController : Controller
    {
        private readonly DbVegetableContext _context;

        public OrderController(DbVegetableContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));  // 添加防止 null 的檢查;
        }
        public IActionResult List()
        {
            var datas = from t in _context.TOrders select t;
            List<COrderWrap> list = new List<COrderWrap>();
            foreach (var t in datas)
                list.Add(new COrderWrap() { order = t });
            return View(list);
        }

        public IActionResult OrderList(int? id)
        {
            if (id == null)
                return RedirectToAction("Order");

            // 先用 .ToList() 把查詢結果載入到記憶體
            List<TOrderList> datas = _context.TOrderLists
                                             .Where(p => p.FOrderId == id)
                                             .ToList();

            List<COrderListWrap> list = new List<COrderListWrap>();
            foreach (var t in datas)
            {
                var product = _context.TProducts.FirstOrDefault(p => p.FId == t.FProductId);
                list.Add(new COrderListWrap() { orderList = t, ProductName = product?.FName });
            }
            return View(list);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int id, int status)
        {
            var order = _context.TOrders.FirstOrDefault(o => o.FId == id);
            if (order == null)
            {
                return Json(new { success = false, message = "訂單不存在" });
            }

            order.FStatus = status;
            _context.SaveChanges();

            return Json(new { success = true });
        }

    }
}
