using Microsoft.AspNetCore.Mvc;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class CarouselController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private const string UploadFolder = "uploads";

        public CarouselController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, UploadFolder);

            // 取得目前資料夾中所有圖片的路徑
            var imagePaths = Directory.GetFiles(uploadsPath)
                                      .Select(path => "/uploads/" + Path.GetFileName(path))
                                      .ToList();

            var model = new CarouselImageViewModel
            {
                ImagePaths = imagePaths
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(CarouselImageViewModel model)
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, UploadFolder);
            Directory.CreateDirectory(uploadsPath);  // 確保圖片資料夾存在

            // 把每個圖片檔案對應到一個具體的名稱（Carousel1.jpg, Carousel2.jpg等）
            var files = new[] { model.Image1, model.Image2, model.Image3 };

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != null && files[i].Length > 0)
                {
                    // 重新命名檔案為 Carousel1.jpg, Carousel2.jpg, ...
                    var fileName = $"Carousel{i + 1}.jpg";
                    var filePath = Path.Combine(uploadsPath, fileName);

                    // 刪除舊圖片（如果有）
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // 儲存新圖片，並將其命名為 Carousel1.jpg, Carousel2.jpg
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await files[i].CopyToAsync(stream);
                    }
                }
            }

            // 取得圖片路徑並加上時間戳
            var imagePaths = Directory.GetFiles(uploadsPath)
                                      .Select(path => $"/uploads/{Path.GetFileName(path)}?t={DateTime.UtcNow.Ticks}")
                                      .ToList();

            var updatedModel = new CarouselImageViewModel
            {
                ImagePaths = imagePaths
            };

            return View(updatedModel);
        }
    }
}