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
            IEnumerable<TPerson> datas = null;
            var data = _context.TPeople.ToList();  
            List<TPersonWrap>list = new List<TPersonWrap>();
            foreach (var p in data) 
            {
                list.Add(new TPersonWrap() { person = p });
            }
            return View(list);
        }

        // GET: TPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPerson = await _context.TPeople
                .FirstOrDefaultAsync(m => m.FId == id);
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
        public async Task<IActionResult> Create(TPersonWrap tPersonwrap)
        {
            // 將新的 TPerson 物件加入到資料庫上下文
            _context.TPeople.Add(tPersonwrap.person);

            // 儲存變更
            await _context.SaveChangesAsync();

            // 重定向回 Index 頁面
            return RedirectToAction(nameof(Index));

            
        }

        // GET: TPersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var x = await _context.TPeople.FirstOrDefaultAsync(c => c.FId == id);
            if (x == null)
                return RedirectToAction(nameof(Index));

            return View(new TPersonWrap() { person = x });
        }

        // POST: TPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(TPersonWrap tPersonwrap)
        {
            TPerson e = _context.TPeople.FirstOrDefault(c => c.FId == tPersonwrap.FId);
            
            if (e != null)
            {   //拿掉不給使用者修改的欄位
               e.FName = tPersonwrap.FName;
               e.FAccount = tPersonwrap.FAccount;
               e.FPassword = tPersonwrap.FPassword;
               e.FBirth =  tPersonwrap.FBirth;
               e.FPhone = tPersonwrap.FPhone;
               e.FTel = tPersonwrap.FTel;
               e.FAddress = tPersonwrap.FAddress;
               e.FEmail = tPersonwrap.FEmail;
               e.FUbn = tPersonwrap.FUbn;
               e.FPermissiion = tPersonwrap.FPermissiion;
               e.FEmp = tPersonwrap.FEmp;
               e.FEmpTel  = tPersonwrap.FEmpTel;
               e.FCreatedAt = tPersonwrap.FCreatedAt;
               e.FEditor = tPersonwrap.FEditor;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TPersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPerson = await _context.TPeople
                .FirstOrDefaultAsync(m => m.FId == id);
            if (tPerson == null)
            {
                return NotFound();
            }
            _context.TPeople.Remove(tPerson);
            _context.SaveChanges();

            // 重定向回 Index 頁面
            return RedirectToAction(nameof(Index));
        }

        

        private bool TPersonExists(int id)
        {
            return _context.TPeople.Any(e => e.FId == id);
        }
    }
}
