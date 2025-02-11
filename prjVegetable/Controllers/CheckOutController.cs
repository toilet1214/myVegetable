using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace prjVegetable.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly DbVegetableContext _dbContext;
        int totalamount = 0;

        public ActionResult CheckOutIndex()
        {
            return View();
        }
        //step1 : 網頁導入傳值到前端
        public ActionResult Payment()
        {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //需填入你的網址
            var website = $"https://localhost:7251";
            var order = new Dictionary<string, string>
            {
            //綠界需要的參數
            { "MerchantTradeNo",  orderId},
            { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
            { "TotalAmount",  "100"},
            { "TradeDesc",  "無"},
            { "ItemName",  "測試商品"},
            { "ExpireDate",  "3"},
            { "CustomField1",  ""},
            { "CustomField2",  ""},
            { "CustomField3",  ""},
            { "CustomField4",  ""},
            { "ReturnURL",  $"{website}/api/AddPayInfo"},
            { "OrderResultURL", $"{website}/CheckOut/PayInfo/{orderId}"},
            { "PaymentInfoURL",  $"{website}/api/AddAccountInfo"},
            { "ClientRedirectURL",  $"{website}/CheckOut/AccountInfo/{orderId}"},
            { "MerchantID",  "2000132"},
            { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
            { "PaymentType",  "aio"},
            { "ChoosePayment",  "ALL"},
            { "EncryptType",  "1"},
            };
            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);
            return View(order);
        }
        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";
            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";
            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        [HttpPost]
        public ActionResult PayInfo(IFormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            DbVegetableContext db = new DbVegetableContext();
            string temp = id["MerchantTradeNo"]; //寫在LINQ(下一行)會出錯，
            var ecpayOrder = db.EcpayOrders.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
            if (ecpayOrder != null)
            {
                ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
                if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
                ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
                ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
                db.SaveChanges();
            }
            return View("CheckOutIndex", data);
        }
        /// step5 : 取得虛擬帳號 資訊
        [HttpPost]
        public ActionResult AccountInfo(IFormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            DbVegetableContext db = new DbVegetableContext();
            string temp = id["MerchantTradeNo"]; //寫在LINQ會出錯
            var ecpayOrder = db.EcpayOrders.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
            if (ecpayOrder != null)
            {
                ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
                if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
                ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
                ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
                db.SaveChanges();
            }
            return View("CheckOutIndex", data);
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

                return RedirectToAction("Payment", "CheckOut", new { orderId = newOrder.FId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("錯誤：" + ex.Message);
                return View("Error");
            }
        }
    }
}
