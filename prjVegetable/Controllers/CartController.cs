using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly DbVegetableContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public CartController(ILogger<CartController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = environment;
        }

        // 1. 統一用此方法「直接從資料庫」抓購物車資料
        private List<CCartWrap> GetCartInfo()
        {
            if (!Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
            {
                return new List<CCartWrap>(); // 未登入直接回傳空清單
            }

            var cartDatas = _dbContext.TCarts
                .Where(c => c.FPersonId == userId)
                .ToList();

            List<CCartWrap> cartWrapList = new List<CCartWrap>();
            foreach (var cartData in cartDatas)
            {
                var productData = _dbContext.TProducts.FirstOrDefault(p => p.FId == cartData.FProductId);
                var imgData = _dbContext.TImgs.FirstOrDefault(img => img.FProductId == cartData.FProductId);

                cartWrapList.Add(new CCartWrap
                {
                    Cart = cartData,
                    FPrice = productData?.FPrice ?? 0,
                    FCount = cartData.FCount,
                    FProductName = productData?.FName ?? "",
                    FName = imgData?.FName ?? ""
                });
            }
            return cartWrapList;
        }

        // 2. Cart 頁面：呼叫 GetCartInfo() 即可
        public IActionResult Cart()
        {
            var cartWrapList = GetCartInfo();
            int totalPrice = cartWrapList.Sum(item => item.FPrice * item.FCount);
            ViewBag.TotalPrice = totalPrice;
            return View(cartWrapList);
        }

        // 3. 移除購物車商品：直接刪除 TCart 裏的資料
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            if (!Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
                return RedirectToAction("Index", "Home");

            var cartItem = _dbContext.TCarts
                .FirstOrDefault(c => c.FPersonId == userId && c.FProductId == productId);

            if (cartItem != null)
            {
                _dbContext.TCarts.Remove(cartItem);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Cart");
        }

        // 4. 增加數量
        [HttpPost]
        public IActionResult IncreaseQuantity(int productId)
        {
            if (!Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
                return RedirectToAction("Index", "Home");

            var cartItem = _dbContext.TCarts
                .FirstOrDefault(c => c.FPersonId == userId && c.FProductId == productId);

            if (cartItem != null)
            {
                cartItem.FCount += 1;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Cart");
        }

        // 5. 減少數量
        [HttpPost]
        public IActionResult DecreaseQuantity(int productId)
        {
            if (!Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
                return RedirectToAction("Index", "Home");

            var cartItem = _dbContext.TCarts
                .FirstOrDefault(c => c.FPersonId == userId && c.FProductId == productId);

            if (cartItem != null)
            {
                if (cartItem.FCount > 1)
                    cartItem.FCount -= 1;
                else
                    _dbContext.TCarts.Remove(cartItem);

                _dbContext.SaveChanges();
            }
            return RedirectToAction("Cart");
        }

        // 6. 取得會員資訊（原邏輯不動）
        [HttpGet]
        public IActionResult GetMemberInfo()
        {
            if (!Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int memberId))
            {
                return Json(new { success = false, message = "用戶未登入。" });
            }
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

        // 7. 結帳：直接從資料庫抓取購物車資料
        [HttpPost]
        public IActionResult Checkout(string? shippingName, string? shippingPhone, string? shippingAddress, string? note)
        {
            if (string.IsNullOrEmpty(shippingName) || string.IsNullOrEmpty(shippingPhone) || string.IsNullOrEmpty(shippingAddress))
                return BadRequest("表單提交的資料不完整。");

            if (!Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int currentUserId))
                return RedirectToAction("Index", "Home");

            try
            {
                // 從資料庫抓購物車
                var cartItems = _dbContext.TCarts
                    .Where(c => c.FPersonId == currentUserId)
                    .ToList();

                if (!cartItems.Any())
                    return BadRequest("購物車是空的，無法完成結帳。");

                // 建立 TOrder
                var newOrder = new TOrder
                {
                    FPersonId = currentUserId,
                    FStatus = 0,
                    FOrderAt = DateTime.Now,
                    FAddress = shippingAddress,
                    FReceiverName = shippingName,
                    FPhone = shippingPhone,
                    FNote = note
                };
                _dbContext.TOrders.Add(newOrder);
                _dbContext.SaveChanges();

                // 建立 TOrderList
                foreach (var cartItem in cartItems)
                {
                    var orderListItem = new TOrderList
                    {
                        FOrderId = newOrder.FId,
                        FProductId = cartItem.FProductId
                    };
                    _dbContext.TOrderLists.Add(orderListItem);
                }
                _dbContext.SaveChanges();

                // 清空購物車（直接刪除 TCart）
                _dbContext.TCarts.RemoveRange(cartItems);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", new { orderId = newOrder.FId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("錯誤：" + ex.Message);
                return View("Error");
            }
        }
    }
}
