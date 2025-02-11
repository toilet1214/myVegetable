﻿using Microsoft.AspNetCore.Mvc;
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
            IEnumerable<TOrderList> datas = null;
            datas = _context.TOrderLists.Where(p => p.FOrderId == id);
            List<COrderListWrap> list = new List<COrderListWrap>();
            foreach (var t in datas)
                list.Add(new COrderListWrap() { orderList = t });
            return View(list);
        }


    }
}
