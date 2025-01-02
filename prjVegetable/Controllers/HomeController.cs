using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<CProductWrap> list = new List<CProductWrap>();
            var products = db.TProducts.ToList();
            //var images = db.TImgs.ToList();

            foreach (var p in products)
            {
                CProductWrap pp = new CProductWrap() { product  = p};
                var image = db.TImgs.FirstOrDefault(img => img.FProductId == pp.FId && img.FOrderBy == 1);
                pp.FImgName = image?.FImgName;
                list.Add(pp);
            }

            return View(list);
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
