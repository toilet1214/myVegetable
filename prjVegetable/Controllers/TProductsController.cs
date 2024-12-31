using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class TProductsController : Controller
    {
        private readonly DbVegetableContext _context;

        public TProductsController(DbVegetableContext context)
        {
            _context = context;
        }

        // GET: TProducts
        public async Task<IActionResult> Index(CKeywordViewModel vm)
        {
            string keyword = vm.txtKeyword;
            IEnumerable<TProduct> datas = null;
            if (string.IsNullOrEmpty(keyword))
            {
                datas = from p in _context.TProducts
                        select p;
            }
            else 
            {
                datas = _context.TProducts.Where(p =>
                p.FName.Contains(keyword)||
                p.FClassification.Contains(keyword) ||
                p.FLaunchAt.ToString().Contains(keyword) ||
                p.FStorage.Contains(keyword) ||
                p.FOrigin.Contains(keyword)
                );
            }
            var data = _context.TProducts.ToList();
            List<TProductWrap>list = new List<TProductWrap>();
            foreach (var p in data) 
            {
                list.Add(new TProductWrap() { product = p });
            
            }
            return View(list);
        }

        // GET: TProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var tProduct = await _context.TProducts
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tProduct == null)
            {
                return NotFound();
            }

            return View(tProduct);
        }

        // GET: TProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FId,FName,FClassification,FPrice,FDescription,FQuantity,FLaunchAt,FStorage,FOrigin,FEditer")] TProduct tProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tProduct);
        }

        // GET: TProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tProduct = await _context.TProducts.FindAsync(id);
            if (tProduct == null)
            {
                return NotFound();
            }
            return View(tProduct);
        }

        // POST: TProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FId,FName,FClassification,FPrice,FDescription,FQuantity,FLaunchAt,FStorage,FOrigin,FEditer")] TProduct tProduct)
        {
            if (id != tProduct.FId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TProductExists(tProduct.FId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tProduct);
        }

        // GET: TProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tProduct = await _context.TProducts
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tProduct == null)
            {
                return NotFound();
            }

            return View(tProduct);
        }

        // POST: TProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tProduct = await _context.TProducts.FindAsync(id);
            if (tProduct != null)
            {
                _context.TProducts.Remove(tProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TProductExists(int id)
        {
            return _context.TProducts.Any(e => e.FId == id);
        }
    }
}
