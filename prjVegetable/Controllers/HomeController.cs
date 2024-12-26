using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.Diagnostics;

namespace prjVegetable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
            DbVegetableContext db = new DbVegetableContext();
            var products = db.TProducts.ToList();
            var images = db.TImgs.ToList();
            foreach (var product in products)
            {
                var image = product.TImgs.FirstOrDefault(img => img.FProductId == product.FProductId);
                product.FImgName = image?.FImgName;

            }
            return View(products);
        }

        public IActionResult Privacy()
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
