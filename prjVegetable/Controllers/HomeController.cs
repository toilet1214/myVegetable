using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.Diagnostics;

namespace prjVegetable.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
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

        public IActionResult ProductList()
        {
            return View();
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
