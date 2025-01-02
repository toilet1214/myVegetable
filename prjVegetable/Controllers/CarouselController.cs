using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class CarouselController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public CarouselController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(CarouselImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var files = new[] { model.Image1, model.Image2, model.Image3 };
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                        Directory.CreateDirectory(uploads); // 確保目錄存在
                        var filePath = Path.Combine(uploads, Path.GetFileName(file.FileName));
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
                return RedirectToAction("Upload");
            }
            return View(model);
        }
    }
}
