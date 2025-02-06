using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using prjVegetable.ViewModels;
using Microsoft.Data.SqlClient;

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
            // **先將資料轉換為 List，確保 EF Core 連線已經關閉**
            var dataList = datas.ToList();

            // **查詢 TPerson 和 TProvider 並轉為字典，避免多次查詢**
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
            var viewModel = new CGoodsInAndOutViewModel
            {
                GoodsInAndOut = new TGoodsInAndOut(),
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

            // **1️⃣ 轉換 CGoodsInAndOutViewModel 為 TGoodsInAndOut (主表)**
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

            // **2️⃣ 儲存主表並獲取 FId**
            _dbContext.TGoodsInAndOuts.Add(goodsInAndOut);
            _dbContext.SaveChanges(); // 這裡確保 FId 產生

            int generatedFId = goodsInAndOut.FId; // 獲取主表 FId

            // **3️⃣ 轉換並插入 TGoodsInAndOutDetail (明細表)**
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

                _dbContext.TGoodsInAndOutDetails.AddRange(details); // **批量插入**
                _dbContext.SaveChanges();
            }

            return Json(new { success = true, fId = generatedFId });
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
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsInAndOutWrap = _dbContext.Set<CGoodsInAndOutWrap>().Find(id);
            if (goodsInAndOutWrap == null)
            {
                return NotFound();
            }
            return View(goodsInAndOutWrap);
        }

        // POST: GoodsInAndOut/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CGoodsInAndOutWrap goodsInAndOutWrap)
        {
            if (id != goodsInAndOutWrap.FId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Update(goodsInAndOutWrap);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(goodsInAndOutWrap);
        }

        // GET: GoodsInAndOut/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsInAndOutWrap = _dbContext.Set<CGoodsInAndOutWrap>().FirstOrDefault(m => m.FId == id);
            if (goodsInAndOutWrap == null)
            {
                return NotFound();
            }
            return View(goodsInAndOutWrap);
        }

        // GET: GoodsInAndOut/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tGoods = await _dbContext.TGoodsInAndOuts
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tGoods == null)
            {
                return NotFound();
            }
            _dbContext.TGoodsInAndOuts.Remove(tGoods);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
