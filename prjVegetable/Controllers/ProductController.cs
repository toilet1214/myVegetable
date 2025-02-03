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
        public IActionResult ProductList(ProductViewModel pvm, int page =1)
        {
            DbVegetableContext db = new DbVegetableContext();
            List<CProductWrap> list = new List<CProductWrap>();
            string keyword = pvm.category;

            // 設置 ViewBag.Category，用來在麵包屑顯示
            ViewBag.Category = string.IsNullOrEmpty(keyword) ? "產品列表" : keyword;

            IEnumerable<TProduct> datas = null;

            // 取得篩選的最低價格和最高價格
            //decimal精確數值的資料型別，小數點後兩位
            decimal? minPrice = pvm.MinPrice;
            decimal? maxPrice = pvm.MaxPrice;

            //篩選分類
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

            //頁碼排序
            int pagesize = 10;
            int totalProducts = datas.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pagesize);
            datas = datas.Skip((page - 1) * pagesize).Take(pagesize);


            foreach (var p in products)     
            {
                CProductWrap pp = new CProductWrap() { product = p };
                var image = db.TImgs.FirstOrDefault(img => img.FProductId == pp.FId && img.FOrderBy == 1);
                pp.FImgName = image?.FName;
                list.Add(pp);
            }
           

            return View(list);
        }

        //public ActionResult Index(int page = 1)
        //{
        //    int pageSize = 10; // 每頁顯示10筆
        //    int skipCount = (page - 1) * pageSize; // 計算要跳過的數量

        //    // 取得產品列表並進行分頁
        //    var products = _context.TProducts
        //                            .Skip(skipCount)
        //                            .Take(pageSize)
        //                            .ToList();

        //    // 總產品數量
        //    int totalProducts = _context.TProducts.Count();

        //    // 計算總頁數
        //    int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

        //    // 傳遞分頁資料到視圖
        //    ViewBag.TotalPages = totalPages;
        //    ViewBag.CurrentPage = page;

        //    return View(products);
        //}






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
