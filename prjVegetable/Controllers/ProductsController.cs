﻿using System;
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
                .OrderBy(i=>i.FOrderBy)
                .Select(i => new 
                { 
                    i.FId,
                    i.FProductId,
                    i.FName,
                    i.FOrderBy,
                    i.FUploadAt,
                    i.FEditor
                })
                .ToListAsync();

            var imagesNames = images.Select(i => i.FName).ToList();

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
                Images = imagesNames, // 確保返回空列表而不是 null
                AllImageData = images
            };

            return Ok(productWithImages);
        }        

        [HttpPut]
        public async Task<IActionResult> update(int id,[FromBody] CProductWrap productwrap)
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
        public async Task<IActionResult> updateImage([FromForm] IFormFile file, [FromForm]int index)
        {            
            if (file == null || index < 0)
            {
                return BadRequest("無效的圖片索引");
            }
            try
            {
                // 你可以將檔案儲存到伺服器的某個資料夾中
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

                // 確保資料夾存在
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 產生檔案儲存的完整路徑
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                // 儲存檔案
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 查詢目前已經存在的圖片並取得其 FOrderBy
                var images = await _context.TImgs
                    .Where(img => img.FProductId == 1) // 假設產品 ID 是 1，您可以根據需求動態取得此 ID
                    .OrderBy(img => img.FOrderBy)
                    .ToListAsync();

                // 確保索引有效
                if (index >= images.Count)
                {
                    return BadRequest("無效的圖片索引");
                }

                // 取得當前圖片以及要更新的圖片
                var currentImage = images[index];
                var currentOrderBy = currentImage.FOrderBy;  // 儲存目前圖片的 FOrderBy

                foreach (var image in images.Where(i=>i.FOrderBy>=currentOrderBy &&i.FOrderBy!= currentImage.FOrderBy)) { image.FOrderBy++; }
                
                Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);

                var productImage = new TImg
                {
                    FProductId = 1,//暫時固定，要再依照前端資料調整
                    FName=file.FileName,
                    FOrderBy= currentOrderBy,//需要與前端圖片的orderby更換
                    FUploadAt = DateOnly.FromDateTime(DateTime.Now),
                    FEditor= UserId//需要取得現在使用者
                };
                _context.TImgs.Add(productImage);

                // 更新其他圖片的 FOrderBy
                _context.TImgs.UpdateRange(images);
                await _context.SaveChangesAsync();

                var updatedImages = await _context.TImgs
                    .Where(img => img.FProductId == 1)
                    .OrderBy(img => img.FOrderBy)
                    .Select(img => img.FName)
                    .ToListAsync();

                // 在這裡回傳成功訊息
                return Ok(new { FilePath = file.FileName, Images= updatedImages });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"內部伺服器錯誤: {ex.Message}");
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

