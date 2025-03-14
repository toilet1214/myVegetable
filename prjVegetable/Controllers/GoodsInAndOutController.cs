﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using prjVegetable.ViewModels;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace prjVegetable.Controllers
{
    public class GoodsInAndOutController : Controller
    {
        private readonly ILogger<GoodsInAndOutController> _logger;
        private readonly DbVegetableContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public GoodsInAndOutController(ILogger<GoodsInAndOutController> logger, DbVegetableContext dbContext, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _environment = environment;
        }

        // GET: GoodsInAndOut
        public IActionResult GoodsInAndOutIndex(CKeywordViewModel vm)
        {
            string keyword = vm.txtKeyword;
            IEnumerable<TGoodsInAndOut> datas = null;

            if (string.IsNullOrEmpty(keyword))
            {
                datas = _dbContext.TGoodsInAndOuts;
            }
            else
            {
                // 先從 TPerson 和 TProvider 查詢對應的 FId
                var personIds = _dbContext.TPeople
                    .Where(p => p.FName.Contains(keyword))
                    .Select(p => p.FId)
                    .ToList();

                var providerIds = _dbContext.TProviders
                    .Where(p => p.FName.Contains(keyword))
                    .Select(p => p.FId)
                    .ToList();

                datas = _dbContext.TGoodsInAndOuts
                    .Where(p =>
                        (LookupDictionary.InOutMapping.ContainsKey(p.FInOut) && LookupDictionary.InOutMapping[p.FInOut].Contains(keyword)) ||
                        p.FId.ToString().Contains(keyword) ||
                        p.FInvoiceId.ToString().Contains(keyword) ||
                        personIds.Contains(p.FPersonId) ||
                        providerIds.Contains(p.FProviderId) ||
                        p.FDate.ToString().Contains(keyword) ||
                        p.FTotal.ToString().Contains(keyword) ||
                        p.FEditor.ToString().Contains(keyword) ||
                        p.FNote.Contains(keyword)
                    );
            }
            // 先將資料轉換為 List，確保 EF Core 連線已經關閉
            var dataList = datas.ToList();

            // 查詢 TPerson 和 TProvider 並轉為字典，避免多次查詢
            var personDict = _dbContext.TPeople
                .ToDictionary(p => p.FId, p => p.FName);

            var providerDict = _dbContext.TProviders
                .ToDictionary(p => p.FId, p => p.FName);
            var result = dataList.Select(d => new CGoodsInAndOutWrap
            {
                GoodsInAndOut = d,
                FInOutText = LookupDictionary.InOutMapping.ContainsKey(d.FInOut) ? LookupDictionary.InOutMapping[d.FInOut] : "未知",
                FPersonName = personDict.ContainsKey(d.FPersonId) ? personDict[d.FPersonId] : "未知",
                FProviderName = providerDict.ContainsKey(d.FProviderId) ? providerDict[d.FProviderId] : "未知"
            }).ToList();

            return View(result);
        }



        // GET: GoodsInAndOut/Create
        [HttpGet]
        public IActionResult Create()
        {
            int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId);
            var viewModel = new CGoodsInAndOutViewModel
            {
                GoodsInAndOut = new TGoodsInAndOut
                {
                    FEditor = userId
                },
                GoodsInAndOutDetails = new List<TGoodsInAndOutDetail> { new TGoodsInAndOutDetail() }
            };
            return View(viewModel);
        }

        // POST: GoodsInAndOut/Create (主表)
        [HttpPost]
        public IActionResult Create(CGoodsInAndOutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"欄位: {error.Key}, 錯誤訊息: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Json(new { success = false, message = "主表驗證失敗，請檢查 Console 訊息" });
            }

            // 轉換 CGoodsInAndOutViewModel 為 TGoodsInAndOut (主表)
            var goodsInAndOut = new TGoodsInAndOut
            {
                FInOut = model.GoodsInAndOut.FInOut,
                FDate = model.GoodsInAndOut.FDate,
                FInvoiceId = model.GoodsInAndOut.FInvoiceId,
                FProviderId = model.GoodsInAndOut.FProviderId,
                FPersonId = model.GoodsInAndOut.FPersonId,
                FTotal = model.GoodsInAndOut.FTotal,
                FEditor = model.GoodsInAndOut.FEditor,
                FNote = model.GoodsInAndOut.FNote
            };

            // 儲存主表並獲取 FId
            _dbContext.TGoodsInAndOuts.Add(goodsInAndOut);
            _dbContext.SaveChanges(); // 這裡確保 FId 產生

            int generatedFId = goodsInAndOut.FId; // 獲取主表 FId

            //轉換並插入 TGoodsInAndOutDetail (明細表)
            if (model.GoodsInAndOutDetails != null && model.GoodsInAndOutDetails.Count > 0)
            {
                List<TGoodsInAndOutDetail> details = model.GoodsInAndOutDetails.Select(detail => new TGoodsInAndOutDetail
                {
                    FGoodsInandOutId = generatedFId, // 綁定主表 FId
                    FProductId = detail.FProductId,
                    FCount = detail.FCount,
                    FPrice = detail.FPrice,
                    FSum = detail.FSum
                }).ToList();

                _dbContext.TGoodsInAndOutDetails.AddRange(details); // 批量插入
                _dbContext.SaveChanges();
            }

            return Json(new { success = true, fId = generatedFId });
        }

        [HttpGet]
        public IActionResult GetProductsInfo()
        {
            var products = _dbContext.TProducts
                .Select(p => new { p.FId, p.FName,p.FPrice })
                .ToList();
            Console.WriteLine($"取得產品數量：{products.Count}");
            foreach (var prod in products)
            {
                Console.WriteLine($"產品ID: {prod.FId}, 名稱: '{prod.FName}', 單價: {prod.FPrice}");
            }
            return Json(new { success = true, products});
        }

        // POST: GoodsInAndOut/InsertDetail (批量新增明細)
        [HttpPost]
        public IActionResult InsertDetail([FromBody] List<TGoodsInAndOutDetail> details)
        {
            if (details == null || details.Count == 0)
            {
                return Json(new { success = false, message = "沒有明細資料可插入" });
            }

            List<string> valuesList = new List<string>();
            List<object> parameters = new List<object>();
            int paramIndex = 0;

            foreach (var detail in details)
            {
                string valuePlaceholder = $"(@FGoodsInandOutId{paramIndex}, @FProductId{paramIndex}, @FCount{paramIndex}, @FPrice{paramIndex}, @FSum{paramIndex})";
                valuesList.Add(valuePlaceholder);

                parameters.Add(new SqlParameter($"@FGoodsInandOutId{paramIndex}", detail.FGoodsInandOutId));
                parameters.Add(new SqlParameter($"@FProductId{paramIndex}", detail.FProductId));
                parameters.Add(new SqlParameter($"@FCount{paramIndex}", detail.FCount));
                parameters.Add(new SqlParameter($"@FPrice{paramIndex}", detail.FPrice));
                parameters.Add(new SqlParameter($"@FSum{paramIndex}", detail.FSum));

                paramIndex++;
            }

            string sql = "INSERT INTO TGoodsInAndOutDetail (FGoodsInandOutId, FProductId, FCount, FPrice, FSum) VALUES " + string.Join(", ", valuesList);
            _dbContext.Database.ExecuteSqlRaw(sql, parameters.ToArray());

            return Json(new { success = true });
        }


        // GET: GoodsInAndOut/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var main = _dbContext.TGoodsInAndOuts.FirstOrDefault(x => x.FId == id);
            if (main == null)
            {
                return NotFound();
            }
            if (int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
            {
                main.FEditor = userId;
            }
            var details = _dbContext.TGoodsInAndOutDetails
                            .Where(d => d.FGoodsInandOutId == id)
                            .ToList();

            ViewBag.ProductList = _dbContext.TProducts
                                .Select(p => new { p.FId, p.FName, p.FPrice })
                                .Cast<dynamic>()
                                .ToList();

            var viewModel = new CGoodsInAndOutViewModel
            {
                GoodsInAndOut = main,
                GoodsInAndOutDetails = details
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CGoodsInAndOutViewModel model)
        {
            if (model == null || model.GoodsInAndOut == null)
            {
                return Json(new { success = false, message = "提交的資料無效" });
            }

            Console.WriteLine($"收到的 FId: {model.GoodsInAndOut.FId}");

            int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int currentUserId);

            var main = _dbContext.TGoodsInAndOuts.FirstOrDefault(x => x.FId == model.GoodsInAndOut.FId);
            if (main == null)
            {
                return Json(new { success = false, message = $"找不到主表資料，FId: {model.GoodsInAndOut.FId}" });
            }

            main.FInOut = model.GoodsInAndOut.FInOut;
            main.FDate = model.GoodsInAndOut.FDate;
            main.FInvoiceId = model.GoodsInAndOut.FInvoiceId;
            main.FProviderId = model.GoodsInAndOut.FProviderId;
            main.FPersonId = model.GoodsInAndOut.FPersonId;
            main.FTotal = model.GoodsInAndOut.FTotal;
            main.FNote = model.GoodsInAndOut.FNote;
            main.FEditor = currentUserId;

            _dbContext.SaveChanges(); 
            _dbContext.Database.ExecuteSqlRaw("DELETE FROM TGoodsInAndOutDetail WHERE FGoodsInandOutId = @p0", main.FId);

            if (model.GoodsInAndOutDetails != null && model.GoodsInAndOutDetails.Count > 0)
            {
                var sql = new List<string>();
                var parameters = new List<object>();

                for (int i = 0; i < model.GoodsInAndOutDetails.Count; i++)
                {
                    var detail = model.GoodsInAndOutDetails[i];
                    sql.Add($"(@p{i * 5}, @p{i * 5 + 1}, @p{i * 5 + 2}, @p{i * 5 + 3}, @p{i * 5 + 4})");

                    parameters.Add(main.FId);
                    parameters.Add(detail.FProductId);
                    parameters.Add(detail.FCount);
                    parameters.Add(detail.FPrice);
                    parameters.Add(detail.FSum);
                }

                string query = "INSERT INTO TGoodsInAndOutDetail (FGoodsInandOutId, FProductId, FCount, FPrice, FSum) VALUES " + string.Join(", ", sql);
                _dbContext.Database.ExecuteSqlRaw(query, parameters.ToArray()); 
            }

            return Json(new { success = true });
        }


        [HttpGet]
        public IActionResult GetProductPrice(int productId, int count)
        {
            var product = _dbContext.TProducts.FirstOrDefault(p => p.FId == productId);
            if (product == null)
            {
                return Json(new { success = false, message = "找不到商品" });
            }
            int price = product.FPrice;
            int subtotal = price * count;
            return Json(new { success = true, price = price, subtotal = subtotal });
        }


        // GET: GoodsInAndOut/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var main = _dbContext.TGoodsInAndOuts.FirstOrDefault(x => x.FId == id);
            if (main == null)
            {
                return NotFound();
            }

            var details = _dbContext.TGoodsInAndOutDetails
                            .Where(d => d.FGoodsInandOutId == id)
                            .ToList();

            ViewBag.ProductList = _dbContext.TProducts
                                .Select(p => new { p.FId, p.FName })
                                .ToList();

            var viewModel = new CGoodsInAndOutViewModel
            {
                GoodsInAndOut = main,
                GoodsInAndOutDetails = details
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Invalidate([FromBody] TGoodsInAndOut request)
        {
            if (request == null || request.FId == 0)
            {
                return Json(new { success = false, message = "無效的請求" });
            }

            var record = _dbContext.TGoodsInAndOuts.FirstOrDefault(x => x.FId == request.FId);
            if (record == null)
            {
                return Json(new { success = false, message = "找不到該筆資料" });
            }

            if (int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId))
            {
                record.FInOut = 2;  // 設定為作廢
                record.FEditor = userId;
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "無法取得當前使用者" });
        }

    }
}
