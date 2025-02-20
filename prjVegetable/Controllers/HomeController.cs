using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.Services;
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
            // 初始化 Service
            var service = new CarouselService(_dbContext);

            // 熱門商品資料
            var topProducts = service.GetTop9Products();

            // 圖片輪播資料
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            var images = Directory.Exists(uploadsPath)
                ? Directory.GetFiles(uploadsPath)
                    .Select(path => $"/uploads/{Path.GetFileName(path)}?t={DateTime.UtcNow.Ticks}")
                    .ToList()
                : new List<string>();

            // 整合 ViewModel
            var model = new CHomePageViewModel
            {
                TopProducts = topProducts,
                UploadImages = new CHomePageViewModel.CarouselImageViewModel
                {
                    ImagePaths = images
                }
            };

            return View(model);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
