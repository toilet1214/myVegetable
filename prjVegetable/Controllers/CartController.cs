using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using System.Text.Json;

namespace prjVegetable.Controllers
{
    
    public class CartController : Controller
    {
        private const string CartSessionKey = "CartSession";
        private readonly ILogger<CartController> _logger;
        private readonly DbVegetableContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        public CartController(ILogger<CartController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = environment;
        }
        public IActionResult Cart()
        {
            // 從 Session 中取得購物車資料
            var cart = GetCartFromSession();
            //ViewBag.TotalPrice = cart.Sum(item => item.TotalPrice); // 計算總金額
            return View(cart); // 傳遞購物車資料到 View
        }
        private List<CCartWrap> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson)
                ? new List<CCartWrap>()
                : JsonSerializer.Deserialize<List<CCartWrap>>(cartJson);
        }
        private void ClearCartFromSession()
        {
            HttpContext.Session.Remove("CartSession");
        }

        // 將購物車資料保存到 Session
        private void SaveCartToSession(List<CCartWrap> cart)
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
        public IActionResult GetMemberInfo(int? memberId)
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
        public IActionResult Checkout(string? shippingName, string? shippingPhone, string? shippingAddress, string? note)
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
                //int totalAmount = cart.Sum(item => item.FPrice * item.FCount);

                // 1. 建立 TOrder
                var newOrder = new TOrder
                {
                    FPersonId = currentUserId,
                   // FTotal = totalAmount,
                    FStatus = 0,
                    FOrderAt = DateTime.Now,
                    FAddress = shippingAddress,
                    FReceiverName = shippingName,
                    FPhone = shippingPhone,
                    FNote = note
                };

                _dbContext.TOrders.Add(newOrder);
                _dbContext.SaveChanges(); // 儲存 TOrder，取得自動生成的 FId

                // 2. 建立 OrderList
                foreach (var cartItem in cart)
                {
                    var orderListItem = new TOrderList
                    {
                        FOrderId = newOrder.FId, // 關聯剛剛新增的 TOrder
                        FProductId = cartItem.FProductId,
                        //FProductName = cartItem.FProductName,
                        //FPrice = cartItem.FPrice,
                        //FCount = cartItem.FCount,
                        //FSum = cartItem.FPrice * cartItem.FCount
                    };

                    _dbContext.TOrderLists.Add(orderListItem);
                }

                _dbContext.SaveChanges(); // 儲存所有 OrderList 資料

                // 3. 清空購物車
                ClearCartFromSession();

                return RedirectToAction("Index", new { orderId = newOrder.FId });
            }
            catch (Exception ex)
            {
                // 錯誤處理（可記錄日誌）
                Console.WriteLine("錯誤：" + ex.Message);
                return View("Error");
            }
        }
    }
}
