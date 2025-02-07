using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Claims;
using X.PagedList.Extensions;
using X.PagedList;

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
        public IActionResult ProductList(ProductViewModel pvm, int page)
        {
            DbVegetableContext db = new DbVegetableContext();
            List<CProductWrap> list = new List<CProductWrap>();
            string keyword = pvm.category;
            string textkeyword = pvm.keyword;

            // 設置 ViewBag.Category，用來在麵包屑顯示
            ViewBag.Category = string.IsNullOrEmpty(keyword) ? "所有產品" : keyword;

            // 取得篩選的最低價格和最高價格
            //decimal精確數值的資料型別，小數點後兩位
            decimal? minPrice = pvm.MinPrice;
            decimal? maxPrice = pvm.MaxPrice;

            // 基本的資料查詢篩選條件
            IQueryable<TProduct> datas = db.TProducts
                                        .Where(p => p.FLaunch == 1)
                                        .OrderBy(p => p.FId);

            //關鍵字搜尋
            if (!string.IsNullOrEmpty(textkeyword))
            {
                datas = datas.Where(p => p.FName.Contains(textkeyword) || p.FDescription.Contains(textkeyword));
            }

            //篩選分類 category
            if (!string.IsNullOrEmpty(keyword))
            {
                datas = datas.Where(p => p.FClassification.Contains(keyword) || p.FName.Contains(keyword) || p.FDescription.Contains(keyword));
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
                                    .Where(f => f.FPersonId == personId)
                                    .Select(f => f.FProductId)
                                    .ToList();
            var cartProductIds = db.TCarts
                                .Where(c => c.FPersonId == personId)
                                .Select(c => c.FProductId)
                                .ToList();



            //頁碼排序
            int pagesize = 3;
            int totalProducts = datas.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pagesize);
            page = page < 1 ? 1 : page;
            var pagedDatas = datas
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize)
                            .ToList();

            foreach (var p in pagedDatas)
            {
                CProductWrap pp = new CProductWrap()
                {
                    FId = p.FId,
                    FName = p.FName,
                    FClassification = p.FClassification,
                    FPrice = p.FPrice,
                    FDescription = p.FDescription,
                    FQuantity = p.FQuantity,
                    FLaunch = p.FLaunch,
                    FStorage = p.FStorage,
                    FOrigin = p.FOrigin,
                    FEditor = p.FEditor
                };

                // 查找對應的商品圖片
                var image = db.TImgs.FirstOrDefault(img => img.FProductId == p.FId && img.FOrderBy == 1);
                pp.FImgName = image?.FName;  // 如果圖片存在，設定 FImgName

                // 設定該產品是否為收藏
                pp.IsFavorite = favoriteProductIds.Contains(p.FId);

                list.Add(pp);
            }



            //foreach (var p in pagedDatas)
            //{
            //    CProductWrap pp = new CProductWrap() { product = p };
            //    var image = db.TImgs.FirstOrDefault(img => img.FProductId == pp.FId && img.FOrderBy == 1);
            //    pp.FImgName = image?.FName;
            //    pp.IsFavorite = favoriteProductIds.Contains(p.FId);
            //    list.Add(pp);
            //}

            ViewData["TotalPages"] = totalPages;
            ViewData["Page"] = page;
            ViewData["Category"] = keyword;  // 保留篩選條件
            ViewData["Keyword"] = textkeyword;  // 保留關鍵字篩選
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;

            return View(list);
        }





        [HttpGet]
        public JsonResult CheckLogin()
        {
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            bool isLoggedIn = UserId > 0;

            return Json(new { isLoggedIn = isLoggedIn });
        }



        [HttpPost]
        public IActionResult AddToFavorites(int productId)
        {
            var personIdString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID);
            var personId = string.IsNullOrEmpty(personIdString) ? 0 : int.Parse(personIdString);
            var favorite = _context.TFavorites.FirstOrDefault(f=>f.FPersonId ==personId && f.FProductId == productId);

            if (personId == 0)
            {
                return Json(new { success = false, redirectToLogin = true });
            }

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
            //加入我的最愛
            _context.SaveChanges();
            return Json(new {success = true, isFavorite = isFavorite});
        }



        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var personIdString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID);
            var personId = string.IsNullOrEmpty(personIdString) ? 0 : int.Parse(personIdString);

            if (personId == 0)
            {
                return Json(new { success = false, redirectToLogin = true });
            }

            var product = _context.TProducts.FirstOrDefault(p => p.FId == productId);
            if (product == null)
            {
                return Json(new { success = false, message = "商品不存在" });
            }
            _context.TCarts.Add(new TCart
            {
                FPersonId = personId,
                FProductId = productId,
                FCount = quantity 
            });

            // 將商品加入購物車
            
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

            var personIdString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID);
            var personId = string.IsNullOrEmpty(personIdString) ? 0 : int.Parse(personIdString);
            if (personId > 0)
            {
                var isFavorite = _context.TFavorites
                    .Any(f => f.FPersonId == personId && f.FProductId == id);

                x.IsFavorite = isFavorite;
            }
            else
            {
                x.IsFavorite = false; // 未登入的話，顯示為未加入最愛
            }

            return View(x);
        }

        
    }
}
