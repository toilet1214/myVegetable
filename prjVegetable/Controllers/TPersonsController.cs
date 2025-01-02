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
    public class TPersonsController : Controller
    {
        private readonly DbVegetableContext _context;

        public TPersonsController(DbVegetableContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));  // 添加防止 null 的檢查;
        }

        // GET: TPersons
        public IActionResult Index(CKeywordViewModel vm)
        {
            string keyword = vm.txtKeyword;
            IEnumerable<TPerson> datas = null;
            if (string.IsNullOrEmpty(keyword)) 
            {
                datas = from p in _context.TPeople
                        select p;
            }
            else
            {
                datas = _context.TPeople.Where(p => 
                p.FName.Contains(keyword)||
                p.FPhone.Contains(keyword)||
                p.FAddress.Contains(keyword)||
                p.FEmail.Contains(keyword)
                );
            }
            var data = _context.TPeople.ToList();  
            List<CPersonWrap>list = new List<CPersonWrap>();
            foreach (var p in data) 
            {
                list.Add(new CPersonWrap() { person = p });
            }
            return View(list);
        }

        // GET: TPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var x = await _context.TPeople.FirstOrDefaultAsync(c => c.FId == id);
            if (x == null)
                return RedirectToAction(nameof(Index));

            return View(new CPersonWrap() { person = x });
            
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
        public async Task<IActionResult> Create(CPersonWrap tPersonwrap)
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

            return View(new CPersonWrap() { person = x });
        }

        // POST: TPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(CPersonWrap tPersonwrap)
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
               e.FPermission = tPersonwrap.FPermissiion;
               e.FEmp = tPersonwrap.FEmp;
               e.FEmpTel  = tPersonwrap.FEmpTel;
               e.FCreatedAt = tPersonwrap.FCreatedAt;
               e.FEditor = tPersonwrap.FEditor;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        //        // GET: TPersons/Delete/5
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
    }
}
