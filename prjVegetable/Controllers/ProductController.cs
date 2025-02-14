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
            int pagesize = 10;
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
                    FLaunchAt = p.FLaunchAt,
                    FStorage = p.FStorage,
                    FOrigin = p.FOrigin,
                    FEditor = p.FEditor
                };

                // 查找對應的商品圖片
                var image = db.TImgs.FirstOrDefault(img => img.FProductId == p.FId && img.FOrderBy == 1);
                pp.FImgName = image?.FName;

                // 設定該產品是否為收藏
                pp.IsFavorite = favoriteProductIds.Contains(p.FId);

                list.Add(pp);
            }


            ViewData["TotalPages"] = totalPages;
            ViewData["Page"] = page;
            ViewData["Category"] = keyword;
            ViewData["Keyword"] = textkeyword;
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


        //加入我的最愛
        [HttpPost]
        public IActionResult AddToFavorites(int productId)
        {
            var personIdString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID);
            var personId = string.IsNullOrEmpty(personIdString) ? 0 : int.Parse(personIdString);
            var favorite = _context.TFavorites.FirstOrDefault(f => f.FPersonId == personId && f.FProductId == productId);

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
            _context.SaveChanges();
            return Json(new { success = true, isFavorite = isFavorite });
        }


        //加入購物車
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
                         .Where(img => img.FProductId == id)
                         .OrderBy(img => img.FOrderBy)
                         .Select(img => img.FName)
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
                x.IsFavorite = false;
            }


            x.CommentList = GetProductComments(id, personId);
            if (x.CommentList != null)
            {
                foreach (var comment in x.CommentList)
                {
                    comment.PersonName = DisplayName(comment.PersonName); 
                }
            }

            return View(x);
        }

        //顯示商品評論
        private List<CCommentWrap> GetProductComments(int productId, int personId)
        {
            var comments = _context.TComments
               .Where(c => c.FProductId == productId)
               .OrderByDescending(c => c.FId)
               .Select(c => new
               {
                   c.FPersonId,
                   c.FProductId,
                   c.FOrderListId,
                   c.FComment,
                   c.FStar,
                   c.FCreatedAt,
                   c.FId,
                   PersonName = _context.TPeople
                       .Where(p => p.FId == c.FPersonId)
                       .Select(p => p.FName)
                       .FirstOrDefault(),
               })
               .ToList();

            return comments.Select(comment => new CCommentWrap
            {
                FPersonId = comment.FPersonId,
                FProductId = comment.FProductId,
                FOrderListId = comment.FOrderListId,
                FComment = comment.FComment,
                FStar = comment.FStar,
                FCreatedAt = comment.FCreatedAt,
                PersonName = comment.PersonName,
            }).ToList();
        }

        [HttpGet]  //評論分頁 失敗
        public IActionResult GetProductCommentsPaged(int productId, int page = 1, int pageSize = 5)
        {
            var comments = _context.TComments
                .Where(c => c.FProductId == productId)
                .OrderBy(c => c.FCreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    c.FPersonId,
                    c.FProductId,
                    c.FOrderListId,
                    c.FComment,
                    c.FStar,
                    c.FCreatedAt,
                    PersonName = _context.TPeople
                        .Where(p => p.FId == c.FPersonId)
                        .Select(p => p.FName)
                        .FirstOrDefault(),
                })
                .ToList();

            return Json(comments);
        }


        //加入評論  移到會員中心
        //[HttpPost]
        //public IActionResult AddComment(int productId, string comment, int star)
        //{
        //    var personIdString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID);
        //    var personId = string.IsNullOrEmpty(personIdString) ? 0 : int.Parse(personIdString);

        //    if (personId == 0)
        //    {
        //        return Json(new { success = false, message = "請先登入會員才能提交評論。" });
        //    }

        //    var userName = _context.TPeople
        //    .Where(p => p.FId == personId)
        //    .Select(p => p.FName)
        //    .FirstOrDefault();

        //    if (userName == null)
        //    {
        //        return Json(new { success = false, message = "無法找到使用者名稱。" });
        //    }

        //    var review = new TComment
        //    {
        //        FPersonId = personId,
        //        FProductId = productId,
        //        FComment = comment,
        //        FStar = star,
        //    };

        //    _context.TComments.Add(review);
        //    _context.SaveChanges();

        //    return Json(new { success = true });
        //}


        //隱藏姓名
        
        public string DisplayName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            if (name.Length == 1)
            {
                return name;
            }
            if (name.Length == 2)
            {
                return name[0] + "*"; 
            }
            var firstChar = name[0];
            var lastChar = name[name.Length - 1];
            var maskedMiddle = new string('*', name.Length - 2);
            return firstChar + maskedMiddle + lastChar;
        }

    }


}

