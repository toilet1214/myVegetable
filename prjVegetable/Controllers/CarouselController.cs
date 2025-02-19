using Microsoft.AspNetCore.Mvc;
using prjVegetable.ViewModels;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

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
                                      .Select(path => $"/uploads/{Path.GetFileName(path)}?t={DateTime.UtcNow.Ticks}")
                                      .ToList();

            var model = new CHomePageViewModel.CarouselImageViewModel
            {
                ImagePaths = imagePaths
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(CHomePageViewModel.CarouselImageViewModel model)
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);  // 確保資料夾存在

            // 指定輪播圖片大小 (例如 1200x600)
            int targetWidth = 1200;
            int targetHeight = 600;

            var files = new[] { model.Image1, model.Image2, model.Image3, model.Image4, model.Image5 };

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != null && files[i].Length > 0)
                {
                    var fileName = $"Carousel{i + 1}.jpg";
                    var filePath = Path.Combine(uploadsPath, fileName);

                    // 刪除舊圖片（如果有）
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // 處理圖片大小
                    using (var stream = new MemoryStream())
                    {
                        await files[i].CopyToAsync(stream);
                        var originalImage = Image.FromStream(stream);

                        var resizedImage = ResizeImage(originalImage, targetWidth, targetHeight);
                        resizedImage.Save(filePath, ImageFormat.Jpeg);
                    }
                }
            }

            // 更新圖片路徑
            var imagePaths = Directory.GetFiles(uploadsPath)
                                      .Select(path => $"/uploads/{Path.GetFileName(path)}?t={DateTime.UtcNow.Ticks}")
                                      .ToList();

            var updatedModel = new CHomePageViewModel.CarouselImageViewModel
            {
                ImagePaths = imagePaths
            };

            return View(updatedModel);
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            var resizedImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }





    }
}