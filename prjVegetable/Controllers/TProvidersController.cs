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
    public class TProvidersController : Controller
    {
        private readonly DbVegetableContext _context;

        public TProvidersController(DbVegetableContext context)
        {
            _context = context;
        }

        // GET: TProviders
        public async Task<IActionResult> Index(CKeywordViewModel vm)
        {
            string keyword = vm.txtKeyword;
            IEnumerable<TProvider> datas = null;
            if (string.IsNullOrEmpty(keyword))
            {
                datas = from p in _context.TProviders
                        select p;
            }
            else 
            {
                datas = _context.TProviders.Where(p =>
                p.FName.Contains(keyword)||
                p.FUbn.Contains(keyword) ||
                p.FTel.Contains(keyword) ||
                p.FConnect.Contains(keyword) ||
                p.FAddress.Contains(keyword)
                );
            }
            var data = _context.TProviders.ToList();
            List<CProviderWrap> list = new List<CProviderWrap>();
            foreach (var p in data)
            {
                list.Add(new CProviderWrap() { provider = p });
            }
            return View(list);
        }

        // GET: TProviders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var tProvider = await _context.TProviders
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tProvider == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new CProviderWrap() {provider = tProvider });
        }

        // GET: TProviders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TProviders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(CProviderWrap tProviderwrap)
        {
            _context.TProviders.Add(tProviderwrap.provider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));            
        }

        // GET: TProviders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var tProvider = await _context.TProviders.FirstOrDefaultAsync(c => c.FId ==id);
            if (tProvider == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new CProviderWrap() {provider = tProvider });
        }

        // POST: TProviders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CProviderWrap tProviderwrap)
        {
            TProvider e = await _context.TProviders.FirstOrDefaultAsync(c => c.FId == tProviderwrap.FId);
            if (e != null)
            { //拿掉不給使用者修改的欄位
              e.FName = tProviderwrap.FName;
              e.FUbn = tProviderwrap.FUbn;
                e.FTel=tProviderwrap.FTel;
                e.FConnect = tProviderwrap.FConnect;
                e.FAccountant = tProviderwrap.FAccountant;
                e.FAddress = tProviderwrap.FAddress;
                e.FDelivery = tProviderwrap.FDelivery;
                e.FInvoiceadd = tProviderwrap.FInvoiceadd;
                _context.SaveChanges();
            }            
            return RedirectToAction(nameof(Index));
        }

        // GET: TProviders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tProvider = await _context.TProviders
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tProvider == null)
            {
                return NotFound();
            }
            _context.TProviders.Remove(tProvider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }        
    }
}
