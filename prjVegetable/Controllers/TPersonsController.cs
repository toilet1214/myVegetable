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
    public class TPersonsController : Controller
    {
        private readonly DbVegetableContext _context;

        public TPersonsController(DbVegetableContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));  // 添加防止 null 的檢查;
        }        

        // GET: TPersons
        public IActionResult Index()
        {
            var data = _context.TPeople.ToList();
            return View(data);
        }

        // GET: TPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPerson = await _context.TPeople
                .FirstOrDefaultAsync(m => m.FPId == id);
            if (tPerson == null)
            {
                return NotFound();
            }

            return View(tPerson);
        }

        // GET: TPersons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TPersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FPId,FPName,FPAccount,FPPassword,FPGender,FPBirth,FPPhone,FPTel,FPAddress,FPEmail,FPUinvoice,FPStatus,FPEmp,FPTelEmptel,FPCreatedAt,FPEditor")] TPerson tPerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tPerson);
        }

        // GET: TPersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPerson = await _context.TPeople.FindAsync(id);
            if (tPerson == null)
            {
                return NotFound();
            }
            return View(tPerson);
        }

        // POST: TPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FPId,FPName,FPAccount,FPPassword,FPGender,FPBirth,FPPhone,FPTel,FPAddress,FPEmail,FPUinvoice,FPStatus,FPEmp,FPTelEmptel,FPCreatedAt,FPEditor")] TPerson tPerson)
        {
            if (id != tPerson.FPId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tPerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TPersonExists(tPerson.FPId))
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
            return View(tPerson);
        }

        // GET: TPersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPerson = await _context.TPeople
                .FirstOrDefaultAsync(m => m.FPId == id);
            if (tPerson == null)
            {
                return NotFound();
            }

            return View(tPerson);
        }

        // POST: TPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tPerson = await _context.TPeople.FindAsync(id);
            if (tPerson != null)
            {
                _context.TPeople.Remove(tPerson);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TPersonExists(int id)
        {
            return _context.TPeople.Any(e => e.FPId == id);
        }
    }
}
