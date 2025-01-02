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
    public class ProductsController : Controller
    {
        private readonly DbVegetableContext _context;

        public ProductsController(DbVegetableContext context)
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
            List<CProductWrap>list = new List<CProductWrap>();
            foreach (var p in data) 
            {
                list.Add(new CProductWrap() { product = p });
            
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
                return RedirectToAction(nameof(Index));
            }

            return View(new CProductWrap() {product = tProduct });
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
        public async Task<IActionResult> Create(CProductWrap tProductwrap)
        {
            _context.TProducts.Add(tProductwrap.product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var tProduct = await _context.TProducts.FirstOrDefaultAsync(c => c.FId == id);
            if (tProduct == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new CProductWrap() {product = tProduct });
        }

        // POST: TProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(CProductWrap tProductwrap)
        {
            TProduct e = _context.TProducts.FirstOrDefault(c => c.FId == tProductwrap.FId);

            if (e != null)
            {//拿掉不給使用者修改的欄位
               e.FName = tProductwrap.FName;
                e.FClassification = tProductwrap.FClassification;
                e.FPrice = tProductwrap.FPrice;
                e.FDescription = tProductwrap.FDescription;
                e.FQuantity = tProductwrap.FQuantity;
                e.FLaunchAt = tProductwrap.FLaunchAt;
                e.FStorage = tProductwrap.FStorage;
                e.FOrigin = tProductwrap.FOrigin;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
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
            _context.TProducts.Remove(tProduct);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        
    }
}
