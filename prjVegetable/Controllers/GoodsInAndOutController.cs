using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using prjVegetable.ViewModels;

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
                        p.FProductId.ToString().Contains(keyword) ||
                        p.FCount.ToString().Contains(keyword) ||
                        p.FPrice.ToString().Contains(keyword) ||
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: GoodsInAndOut/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CGoodsInAndOutWrap goodsInAndOutWrap)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Set<CGoodsInAndOutWrap>().Add(goodsInAndOutWrap);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(goodsInAndOutWrap);
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
