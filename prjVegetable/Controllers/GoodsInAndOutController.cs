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
                datas = from p in _dbContext.TGoodsInAndOuts
                        select p;
            }
            else
            {
                datas = _dbContext.TGoodsInAndOuts.Where(p =>
                p.FInOut.Contains(keyword) ||
                p.FDate.Contains(keyword) ||
                p.FInvoiceId.Contains(keyword) ||
                p.FProviderId.Contains(keyword) ||
                p.FPersonId.Contains(keyword) ||
                p.FProductId.Contains(keyword) ||
                p.FCount.Contains(keyword) ||
                p.FPrice.Contains(keyword) ||
                p.FTotal.Contains(keyword) ||
                p.FEditor.Contains(keyword) ||
                p.FNote.Contains(keyword) 


                );
            }
            var data = _dbContext.TGoodsInAndOuts.ToList();
            List<CProviderWrap> list = new List<CProviderWrap>();
            foreach (var p in data)
            {
                list.Add(new CProviderWrap() { provider = p });
            }
            return View(list);
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
