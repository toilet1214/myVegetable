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
