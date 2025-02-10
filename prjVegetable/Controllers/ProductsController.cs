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
            var Img = await _context.TImgs.Where(i => i.FId == id).ToListAsync();

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
                FImgNames = Img.Select(img => $"https://localhost:7251/images/{img.FName}").ToList(),  // 生成圖片的完整URL
            };

            return Ok(productsWithImg);
        }
        
        [HttpPut]
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

                if (productwrap.FImgName != null)
                {
                    if (i == null)
                    {
                        // 如果沒有圖片資料，新增圖片
                        i = new TImg
                        {
                            FProductId = productwrap.FId,
                            FName = productwrap.FImgName // 假設圖片是 URL，若是二進位資料則需進一步處理
                        };
                        _context.TImgs.Add(i);
                    }
                    else
                    {
                        // 如果已有圖片，則更新圖片
                        i.FName = productwrap.FImgName;
                    }
                }

                await _context.SaveChangesAsync();
                return Ok("資料已成功更新");
            }

            catch (Exception ex)
            {
                // 捕捉錯誤並回傳具體錯誤訊息
                return StatusCode(500, $"儲存資料時發生錯誤: {ex.Message}");
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
