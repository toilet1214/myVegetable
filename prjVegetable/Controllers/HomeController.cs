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
            // �q Session �����o�ʪ������
            var cart = GetCartFromSession();
            ViewBag.TotalPrice = cart.Sum(item => item.TotalPrice); // �p���`���B
            return View(cart); // �ǻ��ʪ�����ƨ� View
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, string productName, int price, int count, int imgId)
        {
            // �q Session ���o�ʪ���
            var cart = GetCartFromSession();

            // �ˬd�O�_�w���Ӱӫ~
            var existingItem = cart.FirstOrDefault(x => x.FProductId == productId);
            if (existingItem != null)
            {
                existingItem.FCount += count; // ��s�ƶq
            }
            else
            {
                cart.Add(new TCart
                {
                    FId = cart.Count + 1, // ���] ID ���ۼW
                    FProductId = productId,
                    FProductName = productName,
                    FPrice = price,
                    FCount = count,
                    FImgId = imgId,
                    FBuyerId = 0 // �w�] BuyerId�A��ڥi��q�Τ��T���
                });
            }
            // ��s�ʪ����� Session
            SaveCartToSession(cart);

            return RedirectToAction("Cart");
        }
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
        //[HttpPost]
        //public IActionResult Checkout(string shippingName, string shippingPhone, string shippingAddress, string note, bool sameAsMember)
        //{
        //    if (sameAsMember)
        //    {
        //        // �p�G�Ŀ�P�|����ơA�q�|����ƶ�J
        //        shippingName = "�|���m�W"; // �o������������ڷ|�����
        //        shippingPhone = "�|���q��";
        //        shippingAddress = "�|���a�}";
        //    }
        //    else
        //    {
        //        // �p�G���Ŀ�A�ϥδ��檺��ơ]�i��Ӧۭq�ʤH�^
        //        // shippingName, shippingPhone, shippingAddress �w�g�q��汵��
        //    }

        //    // �B�z�q���޿�A�Ҧp�O�s���Ʈw
        //    // ���]�s�J tOrder ��
        //    var order = new Order
        //    {
        //        fReceiverName = shippingName,
        //        fPhone = shippingPhone,
        //        fAddress = shippingAddress,
        //        fRemark = note,
        //        fOrderAt = DateTime.Now,
        //        fStatus = "Pending",
        //        fTotal = 999 // �����`���B�A���Ӧ��ʪ���
        //    };

        //    // TODO: �N�q��O�s���Ʈw

        //    // ��^�T�{����
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
