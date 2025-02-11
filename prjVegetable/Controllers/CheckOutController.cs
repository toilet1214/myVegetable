using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly DbVegetableContext _dbContext;
        public IActionResult CheckOutIndex()
        {
            return View();
        }

        //  結帳：直接從資料庫抓取購物車資料
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

                // 計算購物車中的總金額
                int totalAmount = 0;
                foreach (var cartItem in cartItems)
                {
                    // 從 TProducts 查詢該商品資訊
                    var product = _dbContext.TProducts.FirstOrDefault(p => p.FId == cartItem.FProductId);
                    int price = product != null ? product.FPrice : 0;
                    totalAmount += price * cartItem.FCount;
                }
                // 建立 TOrder
                var newOrder = new TOrder
                {
                    FPersonId = currentUserId,
                    FStatus = 0,
                    FPay = 0,
                    FOrderAt = DateTime.Now,
                    FTotal = totalAmount,
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
                    // 從 TProducts 查詢該商品資訊
                    var product = _dbContext.TProducts.FirstOrDefault(p => p.FId == cartItem.FProductId);

                    var orderListItem = new TOrderList
                    {
                        FOrderId = newOrder.FId,
                        FProductId = cartItem.FProductId,
                        // 使用 product 的欄位取得價格與名稱
                        FPrice = product != null ? product.FPrice : 0,
                        FCount = cartItem.FCount,
                        FSum = (product != null ? product.FPrice : 0) * cartItem.FCount
                    };
                    _dbContext.TOrderLists.Add(orderListItem);
                }
                _dbContext.SaveChanges();



                // 清空購物車（直接刪除 TCart）
                _dbContext.TCarts.RemoveRange(cartItems);
                _dbContext.SaveChanges();

                return RedirectToAction("CheckOutIndex", "CheckOut", new { orderId = newOrder.FId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("錯誤：" + ex.Message);
                return View("Error");
            }
        }
    }
}
