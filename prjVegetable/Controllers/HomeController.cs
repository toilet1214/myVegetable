using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Diagnostics;
using System.Text.Json;

namespace prjVegetable.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<HomeController> _logger;
        private readonly DbVegetableContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = environment;
        }
       
        public IActionResult Index()
        {
            // 讀取圖片檔案路徑
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            var images = Directory.Exists(uploadsPath)
                ? Directory.GetFiles(uploadsPath).Select(f => "/uploads/" + Path.GetFileName(f)).ToList()
                : new List<string>();

            return View(images); // 將圖片路徑清單傳遞到視圖
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

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(TPerson P)
        {
            DbVegetableContext db = new DbVegetableContext();
            db.TPeople.Add(P);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Forgot()
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
                CProductWrap pp = new CProductWrap() { product = p };
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
