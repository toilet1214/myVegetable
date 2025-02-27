using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
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
            int loginType = 1;
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (string.IsNullOrEmpty(userJson))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                TempData["LogoutMessage"] = "請重新登入";
                return RedirectToAction("Index", "Home");
            }
            var user = JsonSerializer.Deserialize<TPerson>(userJson);
            loginType = user.FLoginType;
            ViewBag.LoginType = loginType;
            return View();


            // 將 loginType 送到前端（假設是 Razor）
            
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerById()
        {
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (string.IsNullOrEmpty(userJson))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                TempData["LogoutMessage"] = "請重新登入";
                return RedirectToAction("Index", "Home");
            }
            var user = JsonSerializer.Deserialize<TPerson>(userJson);
            int UserId = user.FId;
            var Customer = await _context.TPeople.FirstOrDefaultAsync(c => c.FId == UserId);
            return Ok(Customer);
        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] CPersonWrap PersonWrap)
        {
            if (PersonWrap == null || PersonWrap.FId == 0)
            {
                return BadRequest("請求資料錯誤，缺少 FId");
            }

            TPerson e = await _context.TPeople.FirstOrDefaultAsync(c => c.FId == PersonWrap.FId);

            if (e == null)
            {
                return NotFound("找不到對應的會員");
            }

            e.FName = PersonWrap.FName;
            e.FAccount = PersonWrap.FAccount;
            e.FPassword = PersonWrap.FPassword;
            e.FBirth = PersonWrap.FBirth;
            e.FPhone = PersonWrap.FPhone;
            e.FTel = PersonWrap.FTel;
            e.FAddress = PersonWrap.FAddress;
            e.FUbn = PersonWrap.FUbn;
            e.FGender = PersonWrap.FGender;

            await _context.SaveChangesAsync();

            return Ok("資料已成功更新");

        }

        public IActionResult Report()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Report(TReport P)
        {
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (string.IsNullOrEmpty(userJson))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                TempData["LogoutMessage"] = "請重新登入";
                return RedirectToAction("Index", "Home");
            }
            var user = JsonSerializer.Deserialize<TPerson>(userJson);
            P.FPersonId = user.FId;
            _context.TReports.Add(P);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Order()
        {
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (string.IsNullOrEmpty(userJson))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                TempData["LogoutMessage"] = "請重新登入";
                return RedirectToAction("Index", "Home");
            }
            var user = JsonSerializer.Deserialize<TPerson>(userJson);
            IEnumerable<TOrder> datas = null;

            datas = _context.TOrders.Where(p => p.FPersonId == user.FId);
            List<COrderWrap> list = new List<COrderWrap>();
            foreach (var t in datas)
                list.Add(new COrderWrap() { order = t });
            return View(list);
        }

        public async Task<IActionResult> OrderDetail(int? id) //orderid
        {
            if (id == null)
                return RedirectToAction("Order");

            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (string.IsNullOrEmpty(userJson))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                TempData["LogoutMessage"] = "請重新登入";
                return RedirectToAction("Index", "Home");
            }
            var user = JsonSerializer.Deserialize<TPerson>(userJson);
            var orderList = _context.TOrderLists.Where(ol => ol.FOrderId == id).ToList();
            var order = _context.TOrders.FirstOrDefault(p => p.FId == id && p.FPersonId == user.FId);

            //IEnumerable<TOrderList> datas = null;
            //datas = db.TOrderLists.Where(p => p.FOrderId == id);

            List<COrderListWrap> list = new List<COrderListWrap>();
            foreach (var t in orderList)
            {
                var hasComment = _context.TComments
                                .Where(c => c.FOrderListId == t.FId && c.FProductId == t.FProductId && c.FPersonId == user.FId)
                                .Any();

                var productName = _context.TProducts
                                .Where(p => p.FId == t.FProductId)
                                .Select(p => p.FName)
                                .FirstOrDefault() ?? "未知商品";
                list.Add(new COrderListWrap()
                {
                    orderList = t,
                    OrderStatus = order.FStatus,
                    HasComment = hasComment,
                    ProductName = productName
                });
            }



            return View(list);
        }

        public async Task<IActionResult> Favorite()
        {
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (string.IsNullOrEmpty(userJson))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                TempData["LogoutMessage"] = "請重新登入";
                return RedirectToAction("Index", "Home");
            }
            var user = JsonSerializer.Deserialize<TPerson>(userJson);
            var list = _context.TFavorites
                .Where(f => f.FPersonId == user.FId)
                .Join(_context.TProducts,
                      favorite => favorite.FProductId,
                      product => product.FId,
                      (favorite, product) => new CFavoriteViewModel
                      {
                          Name = product.FName,
                          Popular = _context.TFavorites.Count(f => f.FProductId == favorite.FProductId),
                          FId = favorite.FId
                      })
                .ToList();

            return View(list);

        }


        public async Task<IActionResult> AddComment(int? id) //orderlistid
        {

            if (id == null)
            {
                return RedirectToAction("OrderDetail");
            }


            var orderList = _context.TOrderLists.FirstOrDefault(ol => ol.FId == id);
            var order = _context.TOrders.FirstOrDefault(o => o.FId == orderList.FOrderId);
            var product = _context.TProducts.FirstOrDefault(p => p.FId == orderList.FProductId);

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
        public async Task<IActionResult> AddComment(TComment c)
        {
            var OId = _context.TOrderLists.Where(p => p.FId == c.FOrderListId).FirstOrDefault().FOrderId;
            c.FStar = c.FStar == 0 ? 5 : c.FStar;

            _context.TComments.Add(c);
            _context.SaveChanges();


            return RedirectToAction("OrderDetail", new { id = OId });

        }

        public async Task<IActionResult> CommentIndex()
        {
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (string.IsNullOrEmpty(userJson))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                TempData["LogoutMessage"] = "請重新登入";
                return RedirectToAction("Index", "Home");
            }
            var user = JsonSerializer.Deserialize<TPerson>(userJson);
            var datas = _context.TComments
                        .Where(c => c.FPersonId == user.FId)
                        .Join(
                            _context.TOrderLists,
                            comment => comment.FOrderListId,
                            orderList => orderList.FId,
                            (comment, orderList) => new { comment, orderList }
                        )
                        .Select(x => new CCommentWrap
                        {
                            FId = x.comment.FId,
                            FComment = x.comment.FComment,
                            FStar = x.comment.FStar,
                            FPersonName = _context.TPeople.Where(pp => pp.FId == x.comment.FPersonId).FirstOrDefault().FName,
                            FProductName = _context.TProducts.Where(p => p.FId == x.comment.FProductId).FirstOrDefault().FName,
                            FOrderId = x.orderList.FOrderId,
                            FCreatedAt = x.comment.FCreatedAt

                        })
                         .OrderByDescending(x => x.FCreatedAt)
                        .ToList();

            return View(datas);
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

            return RedirectToAction(nameof(Favorite));
        }

    }
}
