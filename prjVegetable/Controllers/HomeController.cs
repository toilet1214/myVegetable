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
        private List<TCart> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson)
                ? new List<TCart>()
                : JsonSerializer.Deserialize<List<TCart>>(cartJson);
        }
        private void ClearCartFromSession()
        {
            HttpContext.Session.Remove("CartSession");
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
        [HttpPost]
        public IActionResult Checkout(string shippingName, string shippingPhone, string shippingAddress, string? remark)
        {
            if (string.IsNullOrEmpty(shippingName) || string.IsNullOrEmpty(shippingPhone) || string.IsNullOrEmpty(shippingAddress))
            {
                return BadRequest("表單提交的資料不完整。");
            }

            try
            {
                // 模擬當前使用者的 FId（假設已登入）
                int currentUserId = HttpContext.Session.GetInt32("UserId") ?? 1; // 若無 Session，預設為 1

                // 計算總金額（int）
                var cart = GetCartFromSession(); // 從 Session 中取得購物車
                if (cart == null || !cart.Any())
                {
                    return BadRequest("購物車是空的，無法完成結帳。");
                }
                int totalAmount = cart.Sum(item => item.FPrice * item.FCount);

                // 如果備註為 null 或空，設為 "無"
                if (string.IsNullOrEmpty(remark))
                {
                    remark = "無";
                }

                // 1. 建立 TOrder
                var newOrder = new TOrder
                {
                    FBuyerId = currentUserId,
                    FTotal = totalAmount,
                    FStatus = "未完成",
                    FOrderAt = DateTime.Now,
                    FAddress = shippingAddress,
                    FReceiverName = shippingName,
                    FPhone = shippingPhone,
                    FRemark = remark
                };

                _dbContext.TOrders.Add(newOrder);
                _dbContext.SaveChanges(); // 儲存 TOrder，取得自動生成的 FId

                // 2. 建立 OrderList
                foreach (var cartItem in cart)
                {
                    var orderListItem = new OrderList
                    {
                        FOrderId = newOrder.FId, // 關聯剛剛新增的 TOrder
                        FProductId = cartItem.FProductId,
                        FProductName = cartItem.FProductName,
                        FPrice = cartItem.FPrice,
                        FCount = cartItem.FCount,
                        FSum = cartItem.FPrice * cartItem.FCount
                    };

                    _dbContext.OrderLists.Add(orderListItem);
                }

                _dbContext.SaveChanges(); // 儲存所有 OrderList 資料

                // 3. 清空購物車
                ClearCartFromSession();

                return RedirectToAction("OrderConfirmation", new { orderId = newOrder.FId });
            }
            catch (Exception ex)
            {
                // 錯誤處理（可記錄日誌）
                Console.WriteLine("錯誤：" + ex.Message);
                return View("Error");
            }
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
