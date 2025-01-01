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
            // �q Session �����o�ʪ������
            var cart = GetCartFromSession();
            ViewBag.TotalPrice = cart.Sum(item => item.TotalPrice); // �p���`���B
            return View(cart); // �ǻ��ʪ�����ƨ� View
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

        // �N�ʪ�����ƫO�s�� Session
        private void SaveCartToSession(List<TCart> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromSession();

            // �d���ʪ��������ӫ~
            var itemToRemove = cart.FirstOrDefault(x => x.FProductId == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove); // �q�ʪ����C�������ӫ~
            }

            SaveCartToSession(cart); // ��s Session
            return RedirectToAction("Cart"); // ��^�ʪ�������
        }
        [HttpPost]
        public IActionResult IncreaseQuantity(int productId)
        {
            var cart = GetCartFromSession();

            // �d���ʪ��������ӫ~
            var item = cart.FirstOrDefault(x => x.FProductId == productId);
            if (item != null)
            {
                item.FCount += 1; // �W�[�ƶq
            }

            SaveCartToSession(cart); // ��s Session
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int productId)
        {
            var cart = GetCartFromSession();

            // �d���ʪ��������ӫ~
            var item = cart.FirstOrDefault(x => x.FProductId == productId);
            if (item != null)
            {
                if (item.FCount > 1)
                {
                    // �p�G�ƶq�j�� 1�A�������
                    item.FCount -= 1;
                }
                else
                {
                    // �p�G�ƶq�� 1�A�ѫe�ݽT�{��M�w�O�_�R��
                    cart.Remove(item);
                }
            }

            SaveCartToSession(cart); // ��s Session
            return RedirectToAction("Cart");
        }
        [HttpGet]
        public IActionResult GetMemberInfo(int memberId)
        {
            // ���] dbContext �w�`�J
            var member = _dbContext.TPeople.FirstOrDefault(p => p.FId == memberId);

            if (member == null)
            {
                return Json(new { success = false, message = "�|����Ƥ��s�b�C" });
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
                return BadRequest("��洣�檺��Ƥ�����C");
            }

            try
            {
                // ������e�ϥΪ̪� FId�]���]�w�n�J�^
                int currentUserId = HttpContext.Session.GetInt32("UserId") ?? 1; // �Y�L Session�A�w�]�� 1

                // �p���`���B�]int�^
                var cart = GetCartFromSession(); // �q Session �����o�ʪ���
                if (cart == null || !cart.Any())
                {
                    return BadRequest("�ʪ����O�Ū��A�L�k�������b�C");
                }
                int totalAmount = cart.Sum(item => item.FPrice * item.FCount);

                // �p�G�Ƶ��� null �ΪšA�]�� "�L"
                if (string.IsNullOrEmpty(remark))
                {
                    remark = "�L";
                }

                // 1. �إ� TOrder
                var newOrder = new TOrder
                {
                    FBuyerId = currentUserId,
                    FTotal = totalAmount,
                    FStatus = "������",
                    FOrderAt = DateTime.Now,
                    FAddress = shippingAddress,
                    FReceiverName = shippingName,
                    FPhone = shippingPhone,
                    FRemark = remark
                };

                _dbContext.TOrders.Add(newOrder);
                _dbContext.SaveChanges(); // �x�s TOrder�A���o�۰ʥͦ��� FId

                // 2. �إ� OrderList
                foreach (var cartItem in cart)
                {
                    var orderListItem = new OrderList
                    {
                        FOrderId = newOrder.FId, // ���p���s�W�� TOrder
                        FProductId = cartItem.FProductId,
                        FProductName = cartItem.FProductName,
                        FPrice = cartItem.FPrice,
                        FCount = cartItem.FCount,
                        FSum = cartItem.FPrice * cartItem.FCount
                    };

                    _dbContext.OrderLists.Add(orderListItem);
                }

                _dbContext.SaveChanges(); // �x�s�Ҧ� OrderList ���

                // 3. �M���ʪ���
                ClearCartFromSession();

                return RedirectToAction("OrderConfirmation", new { orderId = newOrder.FId });
            }
            catch (Exception ex)
            {
                // ���~�B�z�]�i�O����x�^
                Console.WriteLine("���~�G" + ex.Message);
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
