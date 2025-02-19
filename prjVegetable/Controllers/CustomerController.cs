using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Collections.Generic;

namespace prjVegetable.Controllers
{
    public class CustomerController : Controller
    {
        private readonly DbVegetableContext _context;

        public CustomerController(DbVegetableContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));  // 添加防止 null 的檢查;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Index(CCustomerWrap p)
        //{
        //    TPerson x = _context.TPeople.FirstOrDefault(c => c.FId == p.FId);
        //    if (x != null)
        //    {
        //        x.FName = p.FName;
        //        x.FBirth = p.FBirth;
        //        x.FEmail = p.FEmail;
        //        x.FPhone = p.FPhone;
        //        x.FTel = p.FTel;
        //        x.FAddress = p.FAddress;
        //        x.FPassword = p.FPassword;
        //        x.FUbn = p.FUbn;
        //        x.FGender = p.FGender;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public async Task<IActionResult> GetCustomerById()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);

            var Customer = await _context.TPeople.FirstOrDefaultAsync(c => c.FId == UserId);

            return Ok(Customer);
        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] CCustomerWrap CustomerWrap)
        {

            TPerson e = _context.TPeople.FirstOrDefault(c => c.FId == CustomerWrap.FId);

            e.FName = CustomerWrap.FName;
            e.FAccount = CustomerWrap.FAccount;
            e.FPassword = CustomerWrap.FPassword;
            e.FBirth = CustomerWrap.FBirth;
            e.FPhone = CustomerWrap.FPhone;
            e.FTel = CustomerWrap.FTel;
            e.FAddress = CustomerWrap.FAddress;            
            e.FUbn = CustomerWrap.FUbn;


            await _context.SaveChangesAsync();

            return Ok("資料已成功更新");

        }






        public IActionResult Report()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Report(TReport P)
        {
            _context.TReports.Add(P);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Order()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            IEnumerable<TOrder> datas = null;

            datas = _context.TOrders.Where(p => p.FPersonId == UserId);
            List<COrderWrap> list = new List<COrderWrap>();
            foreach (var t in datas)
                list.Add(new COrderWrap() { order = t });
            return View(list);
        }

        public IActionResult OrderDetail(int? id)
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

        public IActionResult Favorite()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            
            
            return View();
        }
    }
}
