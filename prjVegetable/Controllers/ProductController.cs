using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using System.Diagnostics;

namespace prjVegetable.Controllers
{
    public class ProductController : Controller
    {
        private readonly DbVegetableContext _context;
        public ProductController(DbVegetableContext context)
        {
            _context = context;
        }

        //商品列表
        public IActionResult ProductList()
        {
            DbVegetableContext db = new DbVegetableContext();
            List<CProductWrap> list = new List<CProductWrap>();
            var products = db.TProducts.ToList();


            foreach (var p in products)
            {
                CProductWrap pp = new CProductWrap() { product = p };
                var image = db.TImgs.FirstOrDefault(img => img.FProductId == pp.FId && img.FOrderBy == 1);
                pp.FImgName = image?.FName;
                list.Add(pp);
            }

            return View(list);
        }

        //篩選 Classification分類
        [HttpGet]
        public IActionResult GetProductsByCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return Content("查無商品！");
            }

            var products = _context.TProducts
                            .Where(p => p.FClassification == category)
                            .ToList();

            return View(products);
        }



        //單項商品頁面
        public IActionResult ProductItem(int id)
        {
            TProduct p = _context.TProducts.FirstOrDefault(product => product.FId == id);
            if (p == null)
            {
                return NotFound();
            }
            CProductWrap x = new CProductWrap() { product = p };

            x.ImgList = _context.TImgs
                         .Where(img => img.FProductId == id)  // 只選擇對應的商品圖片
                         .OrderBy(img => img.FOrderBy)          // 按 Order 排序
                         .Select(img => img.FName)       // 只選擇圖片路徑
                         .ToList();

            return View(x);
        }
    }
}
