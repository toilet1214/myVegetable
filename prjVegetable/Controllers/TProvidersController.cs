using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.TProviders.ToListAsync());
        }

        // GET: TProviders/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(tProvider);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FId,FName,FUbn,FTel,FConnect,FAccountant,FAddress,FDelivery,FInvoiceadd,FEditer")] TProvider tProvider)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tProvider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tProvider);
        }

        // GET: TProviders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tProvider = await _context.TProviders.FindAsync(id);
            if (tProvider == null)
            {
                return NotFound();
            }
            return View(tProvider);
        }

        // POST: TProviders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FId,FName,FUbn,FTel,FConnect,FAccountant,FAddress,FDelivery,FInvoiceadd,FEditer")] TProvider tProvider)
        {
            if (id != tProvider.FId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tProvider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TProviderExists(tProvider.FId))
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
            return View(tProvider);
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

            return View(tProvider);
        }

        // POST: TProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tProvider = await _context.TProviders.FindAsync(id);
            if (tProvider != null)
            {
                _context.TProviders.Remove(tProvider);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TProviderExists(int id)
        {
            return _context.TProviders.Any(e => e.FId == id);
        }
    }
}
