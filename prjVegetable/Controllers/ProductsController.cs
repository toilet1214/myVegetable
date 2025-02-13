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
            if (id == 0)
            {
                return BadRequest("找不到ID");
            }
            var products = await _context.TProducts.FirstOrDefaultAsync(c => c.FId == id);
            if (products == null)
            {
                return NotFound("products not found");
            }
            var Img = await _context.TImgs.Where(i => i.FProductId == id).Select(i=>i.FName).ToListAsync();
            
            var productsWithImg = new 
            {
                products.FId,
                products.FName,
                products.FClassification,
                products.FPrice,
                products.FDescription,
                products.FIntroduction,
                products.FQuantity,
                products.FLaunch,
                products.FStorage,
                products.FOrigin,
                products.FLaunchAt,
                products.FEditor,
                Images = Img,
            };

            return Ok(productsWithImg);
        }


        [HttpPost]
        public async Task<IActionResult> update([FromBody] CProductWrap productwrap)
        {
            TProduct e = _context.TProducts.FirstOrDefault(c => c.FId == productwrap.FId);
            TImg i = _context.TImgs.FirstOrDefault(i => i.FProductId == productwrap.FId);
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

                await _context.SaveChangesAsync();
                return Ok("資料已成功更新");
            }
            catch (Exception ex)
            {
                // 捕捉錯誤並回傳具體錯誤訊息
                return StatusCode(500, $"儲存資料時發生錯誤: {ex.Message}");
            }
        }

        [HttpPost("update-images/{fId}")]
        public async Task<IActionResult> UpdateProductImages(int fId, List<IFormFile> images)
        {
            var product = await _context.TProducts.FirstOrDefaultAsync(p=>p.FId==fId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            CProductWrap productWrap = new CProductWrap
            {
                product = product,
                ImgList = new List<string>()
            };

            var imagesInDb = await _context.TImgs.Where(i => i.FProductId == fId).ToListAsync();
            foreach (var img in imagesInDb)
            {
                productWrap.ImgList.Add(img.FName);
            }

            // 如果有新的图片上传
            if (images != null && images.Count > 0)
            {
                // 使用硬编码路径
                var _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images"); // 直接写入路径

                // 遍历上传的图片，保存到服务器目录
                foreach (var file in images)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(_imageDirectory, fileName);

                        // 保存文件
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // 如果是修改，替换现有的图片
                        if (productWrap.ImgList != null && productWrap.ImgList.Count > 0)
                        {
                            // 可以选择删除原有图片
                            var oldImage = productWrap.ImgList.FirstOrDefault();
                            if (!string.IsNullOrEmpty(oldImage))
                            {
                                var oldImagePath = Path.Combine(_imageDirectory, oldImage);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath); // 删除旧图
                                }
                            }

                            productWrap.ImgList[0] = fileName; // 更新为新的文件名
                        }
                        else
                        {
                            // 如果是新增，直接添加到 ImgList
                            productWrap.ImgList.Add(fileName);
                        }
                    }
                }
            }

            foreach (var imgName in productWrap.ImgList)
            {
                var imgEntity = new TImg
                {
                    FProductId = fId,
                    FName = imgName
                };

                // 在数据库插入新图片记录
                await _context.TImgs.AddAsync(imgEntity);
            }

            // 更新数据库中的图片信息
            await _context.SaveChangesAsync();

            return Ok(new { message = "Images updated successfully." });
        }










        // GET: TProducts/Create
        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(CProductWrap model, IEnumerable<IFormFile> productImages)
        {
           
                // 儲存商品資料到資料庫
                _context.Add(model.product);
                await _context.SaveChangesAsync();

                // 儲存圖片檔案到 wwwroot/Images 資料夾
                if (productImages != null && productImages.Any())
                {
                    var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath); // 確保資料夾存在
                    }

                    int orderBy = 1;
                    foreach (var imgFile in productImages)
                    {
                        var fileName = Path.GetFileName(imgFile.FileName);
                        var filePath = Path.Combine(imagesPath, fileName);

                        // 儲存圖片檔案到 wwwroot/Images 資料夾
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(stream); // 儲存檔案
                        }

                        // 儲存圖片資料到資料庫
                        var tImg = new TImg
                        {
                            FProductId = model.product.FId,  // 關聯商品ID
                            FName = fileName,                // 儲存檔名
                            FOrderBy = orderBy++,            // 設定排序
                            FUploadAt = DateOnly.FromDateTime(DateTime.Now),
                            FEditor = model.product.FEditor // 編輯者
                        };

                        _context.TImgs.Add(tImg); // 儲存圖片資料到資料庫
                    }

                    await _context.SaveChangesAsync();
                }
                // 完成後導向 Index 頁面
                return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CProductWrap tProductwrap, IFormFile[] productImages)
        //{
        //    _context.TProducts.Add(tProductwrap.product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: TProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var tProduct = await _context.TProducts.FirstOrDefaultAsync(c => c.FId == id);
            if (tProduct == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new CProductWrap() { product = tProduct });
        }

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
