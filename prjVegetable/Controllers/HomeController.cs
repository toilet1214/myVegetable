using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using prjVegetable.Models;
using System.Diagnostics;
using System.Text.Json;

namespace prjVegetable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string CartSessionKey = "CartSession";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        [HttpPost]
        public IActionResult AddToCart(int productId, string productName, int price, int count, int imgId)
        {
            // 從 Session 取得購物車
            var cart = GetCartFromSession();

            // 檢查是否已有該商品
            var existingItem = cart.FirstOrDefault(x => x.FProductId == productId);
            if (existingItem != null)
            {
                existingItem.FCount += count; // 更新數量
            }
            else
            {
                cart.Add(new TCart
                {
                    FId = cart.Count + 1, // 假設 ID 為自增
                    FProductId = productId,
                    FProductName = productName,
                    FPrice = price,
                    FCount = count,
                    FImgId = imgId,
                    FBuyerId = 0 // 預設 BuyerId，實際可能從用戶資訊獲取
                });
            }
            // 更新購物車到 Session
            SaveCartToSession(cart);

            return RedirectToAction("Cart");
        }
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
        //[HttpPost]
        //public IActionResult Checkout(string shippingName, string shippingPhone, string shippingAddress, string note, bool sameAsMember)
        //{
        //    if (sameAsMember)
        //    {
        //        // 如果勾選同會員資料，從會員資料填入
        //        shippingName = "會員姓名"; // 這裡應替換為實際會員資料
        //        shippingPhone = "會員電話";
        //        shippingAddress = "會員地址";
        //    }
        //    else
        //    {
        //        // 如果未勾選，使用提交的資料（可能來自訂購人）
        //        // shippingName, shippingPhone, shippingAddress 已經從表單接收
        //    }

        //    // 處理訂單邏輯，例如保存到資料庫
        //    // 假設存入 tOrder 表
        //    var order = new Order
        //    {
        //        fReceiverName = shippingName,
        //        fPhone = shippingPhone,
        //        fAddress = shippingAddress,
        //        fRemark = note,
        //        fOrderAt = DateTime.Now,
        //        fStatus = "Pending",
        //        fTotal = 999 // 模擬總金額，應來自購物車
        //    };

        //    // TODO: 將訂單保存到資料庫

        //    // 返回確認頁面
        //    return RedirectToAction("OrderConfirmation");
        //}
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
