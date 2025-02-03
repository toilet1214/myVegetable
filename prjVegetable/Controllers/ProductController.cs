using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

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
                datas = db.TProducts.Where(p=>p.FLaunch ==1);
            }
            else {
                datas = db.TProducts.Where(p => p.FClassification.Contains(keyword) && p.FLaunch == 1); 
            
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


            var personIdString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID);
            var personId = string.IsNullOrEmpty(personIdString) ? 0 : int.Parse(personIdString);


            var favoriteProductIds = db.TFavorites
                                    .Where(f=>f.FPersonId == personId)
                                    .Select(f=>f.FProductId)
                                    .ToList();
            var cartProductIds = db.TCarts
                                .Where(c=>c.FPersonId==personId)
                                .Select(c=>c.FProductId)
                                .ToList();



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
                pp.IsFavorite = favoriteProductIds.Contains(p.FId);
                pp.IsInCart = cartProductIds.Contains(p.FId);
                list.Add(pp);
            }
           

            return View(list);
        }

        [HttpPost]
        public IActionResult AddToFavorites(int productId)
        {
            var personIdString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID);
            var personId = string.IsNullOrEmpty(personIdString) ? 0 : int.Parse(personIdString);
            var favorite = _context.TFavorites.FirstOrDefault(f=>f.FPersonId ==personId && f.FProductId == productId);

            bool isFavorite = false;

            if (favorite == null)
            {
                _context.TFavorites.Add(new TFavorite { FPersonId = personId, FProductId = productId });
                isFavorite = true;
            }
            else 
            {
                _context.TFavorites.Remove(favorite);
                isFavorite = false;
            }
            _context.SaveChanges();
            return Json(new {success = true, isFavorite = isFavorite});
        }



        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var personIdString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID);
            var personId = string.IsNullOrEmpty(personIdString) ? 0 : int.Parse(personIdString);

            if (personId == 0)
            {
                return Json(new { success = false, message = "請先登入" });
            }

            var product = _context.TProducts.FirstOrDefault(p => p.FId == productId);
            if (product == null)
            {
                return Json(new { success = false, message = "商品不存在" });
            }

            // 檢查是否已經加入購物車，若已存在則不再加入
            var existingCartItem = _context.TCarts.FirstOrDefault(c => c.FPersonId == personId && c.FProductId == productId);
            if (existingCartItem != null)
            {
                return Json(new { success = false, message = "此商品已在購物車中" });
            }

            // 將商品加入購物車
            _context.TCarts.Add(new TCart { FPersonId = personId, FProductId = productId });
            _context.SaveChanges();

            return Json(new { success = true });
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
