using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DbVegetableContext _context;

        public ProductsController(DbVegetableContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: TProducts
        public async Task<IActionResult> Index(CKeywordViewModel vm)
        {
            DbVegetableContext db = new DbVegetableContext();
            string keyword = vm.txtKeyword;
            IEnumerable<TProduct> datas = null;
            if (string.IsNullOrEmpty(keyword))
            {
                datas = from p in _context.TProducts
                        select p;
            }
            else
            {
                datas = _context.TProducts.Where(p =>
                p.FName.Contains(keyword) ||
                p.FClassification.Contains(keyword) ||
                p.FLaunchAt.ToString().Contains(keyword) ||
                p.FOrigin.Contains(keyword)
                );
            }
            var data = datas.ToList();
            List<CProductWrap> list = new List<CProductWrap>();
            foreach (var p in data)
            {
                CProductWrap pp = new CProductWrap() { product = p };
                var image = db.TImgs.Where(img => img.FProductId == p.FId).ToList();
                foreach (var img in image)
                {
                    pp.ImgList.Add(img.FName);
                }
                list.Add(pp);
            }
            return View(list);
        }

        //詳細資料頁面
        public async Task<IActionResult> Details()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById(int? id)
        {
            // 檢查 id 是否有效
            if (id == null || id <= 0)
            {
                return BadRequest("無效的產品 ID");
            }

            // 查詢產品
            var product = await _context.TProducts.FirstOrDefaultAsync(c => c.FId == id);

            // 如果找不到該產品，返回 NotFound
            if (product == null)
            {
                return NotFound("找不到該產品");
            }

            // 查詢該產品對應的圖片名稱
            var images = await _context.TImgs
                .Where(i => i.FProductId == id)
                .Select(i => i.FName)
                .ToListAsync();

            // 如果沒有圖片，設置為空陣列或者 null，根據需求來決定
            var productWithImages = new
            {
                product.FId,
                product.FName,
                product.FClassification,
                product.FPrice,
                product.FDescription,
                product.FIntroduction,
                product.FQuantity,
                product.FLaunch,
                product.FStorage,
                product.FOrigin,
                product.FLaunchAt,
                product.FEditor,
                Images = images ?? new List<string>(), // 確保返回空列表而不是 null
            };

            return Ok(productWithImages);
        }

        [HttpGet]//呼叫圖片用
        public async Task<IActionResult> GetImagesByProductId(int? productId)
        {
            if (productId == null || productId == 0)
            {
                return BadRequest("找不到產品ID");
            }

            // 查找該產品的所有圖片
            var images = await _context.TImgs
                                       .Where(i => i.FProductId == productId)
                                       .Select(i => new { i.FId, i.FName })
                                       .ToListAsync();

            if (images == null || !images.Any())
            {
                return NotFound("該產品沒有圖片");
            }

            return Ok(images);  // 返回圖片列表
        }

        [HttpPut]
        public async Task<IActionResult> update([FromBody] CProductWrap productwrap)
        {
            TProduct e = _context.TProducts.FirstOrDefault(c => c.FId == productwrap.FId);
            
            if (e == null)
            {
                return NotFound("未找到相關商品");
            }
            try
            {
                e.FName = productwrap.FName;
                e.FClassification = productwrap.FClassification;
                e.FPrice = productwrap.FPrice;
                e.FDescription = productwrap.FDescription;
                e.FIntroduction = productwrap.FIntroduction;
                e.FQuantity = productwrap.FQuantity;
                e.FLaunchAt = productwrap.FLaunchAt;
                e.FStorage = productwrap.FStorage;
                e.FOrigin = productwrap.FOrigin;
                e.FLaunch = productwrap.FLaunch;
                e.FEditor = productwrap.FEditor;
                
            }
            catch (Exception ex)
            {
                // 捕捉錯誤並回傳具體錯誤訊息
                return StatusCode(500, $"儲存資料時發生錯誤: {ex.Message}");
            }

            await _context.SaveChangesAsync();
                return Ok("資料已成功更新");
        }

        // HttpPut method for updating product image
        [HttpPut("updateImage")]
        public async Task<IActionResult> updateImage([FromForm] IFormFile file, [FromForm] int productId)
        {
            TImg img = _context.TImgs.FirstOrDefault(e => e.FProductId == productId);

            // Ensure file is provided
            if (file == null || file.Length == 0)
            {
                return BadRequest("未選擇圖片檔案");
            }

            // Check if product exists
            var product = _context.TProducts.FirstOrDefault(c => c.FId == productId);
            if (product == null)
            {
                return NotFound("未找到相關商品");
            }

            try
            {
                // Generate a unique file name using Guid to prevent overwriting
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Define the path to save the image file (assuming storing it in wwwroot/images)
                var filePath = Path.Combine("wwwroot/images", fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Check if there is already an image record for this product
                var existingImage = _context.TImgs.FirstOrDefault(i => i.FProductId == productId);
                if (existingImage == null)
                {
                    // No image exists, create a new record
                    var newImage = new TImg
                    {
                        FProductId = productId,
                        FName = fileName
                    };
                    _context.TImgs.Add(newImage);
                }
                else
                {
                    // Update the existing image record
                    existingImage.FName = fileName;
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok("圖片已成功上傳");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"儲存圖片時發生錯誤: {ex.Message}");
            }
        }




        // GET: TProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(CProductWrap tProductwrap)
        {
            _context.TProducts.Add(tProductwrap.product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TProducts/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var tProduct = await _context.TProducts.FirstOrDefaultAsync(c => c.FId == id);
        //    if (tProduct == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(new CProductWrap() { product = tProduct });
        //}

        // POST: TProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(CProductWrap tProductwrap)
        {
            TProduct e = _context.TProducts.FirstOrDefault(c => c.FId == tProductwrap.FId);

            if (e != null)
            {//拿掉不給使用者修改的欄位
                e.FName = tProductwrap.FName;
                e.FClassification = tProductwrap.FClassification;
                e.FPrice = tProductwrap.FPrice;
                e.FDescription = tProductwrap.FDescription;
                e.FQuantity = tProductwrap.FQuantity;
                e.FLaunchAt = tProductwrap.FLaunchAt;
                e.FStorage = tProductwrap.FStorage;
                e.FOrigin = tProductwrap.FOrigin;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tProduct = await _context.TProducts
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tProduct == null)
            {
                return NotFound();
            }
            _context.TProducts.Remove(tProduct);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


    }
}

