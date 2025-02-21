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
        private readonly IWebHostEnvironment _environment;
        int totalamount = 0;

        public CheckOutController(ILogger<CartController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;  // 確保這裡 dbContext 不為 null
            _environment = environment;
        }
        public ActionResult CheckOutIndex()
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
                    FNote = note,
                    FMerchantTradeNo = ""
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






        //step1 : 網頁導入傳值到前端
        public ActionResult Payment(int orderId)
        {
            // 確保訂單存在
            var SQLorder = _dbContext.TOrders.FirstOrDefault(o => o.FId == orderId);
            if (SQLorder == null)
            {
                return BadRequest("找不到訂單");
            }

            // 產生綠界單號 (GUID)
            var MerchantTradeNo = Guid.NewGuid().ToString("N").Substring(0, 20);
            SQLorder.FMerchantTradeNo = MerchantTradeNo;
            _dbContext.SaveChanges();
            var orderItems = _dbContext.TOrderLists.Where(ol => ol.FOrderId == SQLorder.FId).ToList();

            // 依據商品ID群組，取每組的第一筆資料
            var distinctOrderItems = orderItems
                .GroupBy(ol => ol.FProductId)
                .Select(g => g.FirstOrDefault())
                .ToList();

            List<string> productNames = new List<string>();
            foreach (var orderItem in distinctOrderItems)
            {
                var product = _dbContext.TProducts.FirstOrDefault(p => p.FId == orderItem.FProductId);
                if (product != null)
                {
                    productNames.Add(product.FName.Trim());
                }
            }
            string itemName = string.Join("#", productNames);
            //需填入你的網址
            var website = $"https://localhost:7251";
            var ECPayOrder = new Dictionary<string, string>
            {
            //綠界需要的參數
            { "MerchantTradeNo",  MerchantTradeNo},
            { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
            { "TotalAmount",  SQLorder.FTotal.ToString()},
            { "TradeDesc",  "無"},
            { "ItemName",  itemName},
            { "ExpireDate",  "3"},
            { "CustomField1",  SQLorder.FId.ToString()},
            { "CustomField2",  ""},
            { "CustomField3",  ""},
            { "CustomField4",  ""},
            { "ReturnURL",  $"{website}/api/AddPayInfo"},
            { "OrderResultURL", $"{website}/CheckOut/PayInfo/{MerchantTradeNo}"},
            { "PaymentInfoURL",  $"{website}/api/AddAccountInfo"},
            { "ClientRedirectURL",  $"{website}/CheckOut/AccountInfo/{MerchantTradeNo}"},
            { "MerchantID",  "2000132"},
            { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
            { "PaymentType",  "aio"},
            { "ChoosePayment",  "ALL"},
            { "EncryptType",  "1"},
            };
            //檢查碼
            ECPayOrder["CheckMacValue"] = GetCheckMacValue(ECPayOrder);
            return View(ECPayOrder);
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

                if (int.TryParse(id["CustomField1"], out int orderId))
                {
                    var order = db.TOrders.FirstOrDefault(o => o.FId == orderId);
                    if (order != null)
                    {
                        int rtnCode = int.Parse(id["RtnCode"]);
                        if (rtnCode == 0)
                        {
                            order.FStatus = 3;
                            order.FPay = 1;
                        }
                        else if (rtnCode == 1)
                        {
                            order.FStatus = 1;
                            order.FPay = 2;
                        }
                        db.SaveChanges();
                    }
                }
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

        [HttpPost("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus([FromBody] OrderUpdateModel updateModel)
        {
            // 根據傳入的 orderId 找到訂單
            var order = _dbContext.TOrders.FirstOrDefault(o => o.FId == updateModel.OrderId);
            if (order == null)
            {
                return NotFound("找不到訂單");
            }

            // 根據 rtnCode 更新狀態
            if (updateModel.RtnCode == 0)
            {
                order.FStatus = 3;
                order.FPay = 1;
            }
            else if (updateModel.RtnCode == 1)
            {
                order.FStatus = 1;
                order.FPay = 2;
            }
            _dbContext.SaveChanges();
            return Ok("更新成功");
        }

        public class OrderUpdateModel
        {
            public int OrderId { get; set; }
            public int RtnCode { get; set; }
        }
    }
}
