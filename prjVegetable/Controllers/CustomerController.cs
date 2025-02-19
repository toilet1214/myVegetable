using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Collections.Generic;
using System.Text.Json;

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
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            int loginType = 0; // 預設值

            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonSerializer.Deserialize<TPerson>(userJson);
                loginType = user.FLoginType;
            }

            // 將 loginType 送到前端（假設是 Razor）
            ViewBag.LoginType = loginType;
            return View();
        }

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
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            P.FPersonId = UserId;
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
            {
                var product = _context.TProducts.FirstOrDefault(p=>p.FId == t.FProductId);
                list.Add(new COrderListWrap() { orderList = t , IsComment = false ,ProductName = product.FName});
            }

            return View(list);
        }

        public IActionResult Favorite()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            IEnumerable<TFavorite> datas = null;

            datas = _context.TFavorites.Where(p => p.FPersonId == UserId);
            List<CFavoriteViewModel> list = new List<CFavoriteViewModel>();
            foreach (var t in datas)
                list.Add(new CFavoriteViewModel() {
                    Name = _context.TProducts.Where(p=>p.FId==t.FProductId).Select(p=>p.FName).ToString(),
                    Popular = _context.TFavorites.Count(f=>f.FProductId==t.FProductId),
                    FId = t.FId,
                });
            return View(list);

        }
        public async Task<IActionResult> DeleteFavorite(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Fav = await _context.TFavorites
                .FirstOrDefaultAsync(m => m.FId == id);
            if (Fav == null)
            {
                return NotFound();
            }
            _context.TFavorites.Remove(Fav);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MyComment()
        {
            return View();
        }
        public async Task<IActionResult> CreateComment()
        {
            return View();
        }
    }
}
