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

            // 取得該使用者所有購物車資料
            var cartDatas = _dbContext.TCarts
                .Where(c => c.FPersonId == userId)
                .ToList();

            // 根據 fProductId 分組，並將同一產品的數量加總 
            var groupedCart = cartDatas.GroupBy(c => c.FProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalCount = g.Sum(c => c.FCount),
                    // 取該分組中的第一筆資料作為參考(其他欄位不影響)
                    CartData = g.First()
                })
                .ToList();

            List<CCartWrap> cartWrapList = new List<CCartWrap>();

            foreach (var group in groupedCart)
            {
                // 從 TProducts 查詢該商品資訊
                var productData = _dbContext.TProducts.FirstOrDefault(p => p.FId == group.ProductId);
                // 從 TImgs 查詢該商品的圖片資訊
                var imgData = _dbContext.TImgs.FirstOrDefault(img => img.FProductId == group.ProductId);

                // 如果查不到商品資料，就跳過
                if (productData == null)
                    continue;

                cartWrapList.Add(new CCartWrap
                {
                    // 這邊的 Cart 可以保留該分組中第一筆資料(但 fCount 會用總加總覆蓋)
                    Cart = group.CartData,
                    FPrice = productData.FPrice,
                    FCount = group.TotalCount, // 使用加總後的數量
                    FProductName = productData.FName,
                    FName = imgData != null ? imgData.FName : ""  // 若查不到圖片則預設空字串
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
                return Json(new { success = false, message = "請先登入" });

            var cartItem = _dbContext.TCarts.FirstOrDefault(c => c.FPersonId == userId && c.FProductId == productId);

            if (cartItem != null)
            {
                cartItem.FCount += 1;
                _dbContext.SaveChanges();
            }

            // 計算新的總金額
            int totalPrice = _dbContext.TCarts
                .Where(c => c.FPersonId == userId)
                .Join(_dbContext.TProducts,
                      cart => cart.FProductId,
                      product => product.FId,
                      (cart, product) => new { cart.FCount, product.FPrice })
                .Sum(x => x.FCount * x.FPrice);

            return Json(new { success = true, newCount = cartItem?.FCount ?? 0, totalPrice });
        }

        // 5. 減少數量
        [HttpPost]
        public IActionResult DecreaseQuantity(int productId)
        {
            if (!Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
                return Json(new { success = false, message = "請先登入" });

            var cartItem = _dbContext.TCarts.FirstOrDefault(c => c.FPersonId == userId && c.FProductId == productId);

            if (cartItem != null)
            {
                if (cartItem.FCount > 1)
                {
                    cartItem.FCount -= 1;
                }
                else
                {
                    _dbContext.TCarts.Remove(cartItem);
                }
                _dbContext.SaveChanges();
            }

            // 計算新的總金額
            int totalPrice = _dbContext.TCarts
                .Where(c => c.FPersonId == userId)
                .Join(_dbContext.TProducts,
                      cart => cart.FProductId,
                      product => product.FId,
                      (cart, product) => new { cart.FCount, product.FPrice })
                .Sum(x => x.FCount * x.FPrice);

            return Json(new { success = true, newCount = cartItem?.FCount ?? 0, totalPrice });

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

        
    }
}
