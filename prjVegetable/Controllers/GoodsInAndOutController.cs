using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

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
        public IActionResult GoodsInAndOutIndex(string txtKeyword)
        {
            // 使用 _dbContext.Set<CGoodsInAndOutWrap>() 來取得資料集合
            var query = _dbContext.Set<CGoodsInAndOutWrap>().AsQueryable();

            if (!string.IsNullOrEmpty(txtKeyword))
            {
                query = query.Where(x => x.FNote.Contains(txtKeyword));
                ViewBag.CurrentFilter = txtKeyword;
            }

            var list = query.ToList();
            return View(list); // 傳入的 Model 為 IEnumerable<CGoodsInAndOutWrap>
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
        public IActionResult Delete(int? id)
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

        // POST: GoodsInAndOut/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var goodsInAndOutWrap = _dbContext.Set<CGoodsInAndOutWrap>().Find(id);
            if (goodsInAndOutWrap != null)
            {
                _dbContext.Set<CGoodsInAndOutWrap>().Remove(goodsInAndOutWrap);
                _dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
