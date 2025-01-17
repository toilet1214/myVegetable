using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
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
        public IActionResult ProductList(ProductViewModel pvm)
        {
            DbVegetableContext db = new DbVegetableContext();
            List<CProductWrap> list = new List<CProductWrap>();
            string keyword = pvm.category;
            IEnumerable<TProduct> datas = null;

            // 取得篩選的最低價格和最高價格
            decimal? minPrice = pvm.MinPrice;
            decimal? maxPrice = pvm.MaxPrice;

            if (string.IsNullOrEmpty(keyword))
            {
                datas = db.TProducts;
            }
            else {
                datas = db.TProducts.Where(p => p.FClassification.Contains(keyword)); 
            
            }


            // 根據價格範圍進行篩選
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                datas = datas.Where(p => p.FPrice >= minPrice.Value && p.FPrice <= maxPrice.Value);
            }
            else if (minPrice.HasValue)
            {
                datas = datas.Where(p => p.FPrice >= minPrice.Value);
            }
            else if (maxPrice.HasValue)
            {
                datas = datas.Where(p => p.FPrice <= maxPrice.Value);
            }


            var products = datas.ToList();


            foreach (var p in products)     
            {
                CProductWrap pp = new CProductWrap() { product = p };
                var image = db.TImgs.FirstOrDefault(img => img.FProductId == pp.FId && img.FOrderBy == 1);
                pp.FImgName = image?.FName;
                list.Add(pp);
            }

            return View(list);
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
