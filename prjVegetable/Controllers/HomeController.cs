using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using System.Diagnostics;
using System.Text.Json;

namespace prjVegetable.Controllers
{
    public class HomeController : Controller
    {
        
        private const string CartSessionKey = "CartSession";
        private readonly ILogger<HomeController> _logger;
        private readonly DbVegetableContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DbVegetableContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cart()
        {
            // 從 Session 中取得購物車資料
            var cart = GetCartFromSession();
            ViewBag.TotalPrice = cart.Sum(item => item.TotalPrice); // 計算總金額
            return View(cart); // 傳遞購物車資料到 View
        }
        //[HttpPost]
        //public IActionResult AddToCart(int productId, string productName, int price, int count, int imgId)
        //{
        //    // 從 Session 取得購物車
        //    var cart = GetCartFromSession();

        //    // 檢查是否已有該商品
        //    var existingItem = cart.FirstOrDefault(x => x.FProductId == productId);
        //    if (existingItem != null)
        //    {
        //        existingItem.FCount += count; // 更新數量
        //    }
        //    else
        //    {
        //        cart.Add(new TCart
        //        {
        //            FId = cart.Count + 1, // 假設 ID 為自增
        //            FProductId = productId,
        //            FProductName = productName,
        //            FPrice = price,
        //            FCount = count,
        //            FImgId = imgId,
        //            FBuyerId = 1 // 預設 BuyerId，實際可能從用戶資訊獲取
        //        });
        //    }
        //    // 更新購物車到 Session
        //    SaveCartToSession(cart);

        //    return RedirectToAction("Cart");
        //}
        // 從 Session 取得購物車資料
        private List<TCart> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson)
                ? new List<TCart>()
                : JsonSerializer.Deserialize<List<TCart>>(cartJson);
        }

        // 將購物車資料保存到 Session
        private void SaveCartToSession(List<TCart> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromSession();

            // 查找購物車內的商品
            var itemToRemove = cart.FirstOrDefault(x => x.FProductId == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove); // 從購物車列表中移除商品
            }

            SaveCartToSession(cart); // 更新 Session
            return RedirectToAction("Cart"); // 返回購物車頁面
        }
        [HttpPost]
        public IActionResult IncreaseQuantity(int productId)
        {
            var cart = GetCartFromSession();

            // 查找購物車內的商品
            var item = cart.FirstOrDefault(x => x.FProductId == productId);
            if (item != null)
            {
                item.FCount += 1; // 增加數量
            }

            SaveCartToSession(cart); // 更新 Session
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int productId)
        {
            var cart = GetCartFromSession();

            // 查找購物車內的商品
            var item = cart.FirstOrDefault(x => x.FProductId == productId);
            if (item != null)
            {
                if (item.FCount > 1)
                {
                    // 如果數量大於 1，直接減少
                    item.FCount -= 1;
                }
                else
                {
                    // 如果數量為 1，由前端確認後決定是否刪除
                    cart.Remove(item);
                }
            }

            SaveCartToSession(cart); // 更新 Session
            return RedirectToAction("Cart");
        }
        [HttpGet]
        public IActionResult GetMemberInfo(int memberId)
        {
            // 假設 dbContext 已注入
            var member = _dbContext.TPeople.FirstOrDefault(p => p.FId == memberId);

            if (member == null)
            {
                return Json(new { success = false, message = "會員資料不存在。" });
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    name = member.FName,
                    phone = member.FPhone,
                    address = member.FAddress
                }
            });
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ProductBuying()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
