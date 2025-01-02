using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;

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
            // 假設我們從某個資料夾中載入當前的圖片路徑
            var imagePaths = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "uploads"))
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
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            var files = new[] { model.Image1, model.Image2, model.Image3 };

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != null && files[i].Length > 0)
                {
                    var fileName = $"Carousel{i + 1}.jpg";
                    var filePath = Path.Combine(uploadsPath, fileName);

                    // 刪除舊圖片
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // 儲存新圖片
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await files[i].CopyToAsync(stream);
                    }
                }
            }

            return RedirectToAction("Upload");
        }
    }
}

