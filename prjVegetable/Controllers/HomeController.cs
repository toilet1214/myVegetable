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
        //[HttpPost]
        //public IActionResult AddToCart(int productId, string productName, int price, int count, int imgId)
        //{
        //    // �q Session ���o�ʪ���
        //    var cart = GetCartFromSession();

        //    // �ˬd�O�_�w���Ӱӫ~
        //    var existingItem = cart.FirstOrDefault(x => x.FProductId == productId);
        //    if (existingItem != null)
        //    {
        //        existingItem.FCount += count; // ��s�ƶq
        //    }
        //    else
        //    {
        //        cart.Add(new TCart
        //        {
        //            FId = cart.Count + 1, // ���] ID ���ۼW
        //            FProductId = productId,
        //            FProductName = productName,
        //            FPrice = price,
        //            FCount = count,
        //            FImgId = imgId,
        //            FBuyerId = 1 // �w�] BuyerId�A��ڥi��q�Τ��T���
        //        });
        //    }
        //    // ��s�ʪ����� Session
        //    SaveCartToSession(cart);

        //    return RedirectToAction("Cart");
        //}
        // �q Session ���o�ʪ������
        private List<TCart> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson)
                ? new List<TCart>()
                : JsonSerializer.Deserialize<List<TCart>>(cartJson);
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
