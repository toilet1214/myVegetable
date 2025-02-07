using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class PersonController : Controller
    {
        private readonly DbVegetableContext _context;

        public PersonController(DbVegetableContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));  // 添加防止 null 的檢查;
        }

        // GET: TPersons
        public IActionResult Index()
        {
            return View();
        }
        //API:獲取所有人員資料
        [HttpGet]
        public async Task<IEnumerable<CPersonWrap>> GetPeople()
        {


            // 確保使用 ToListAsync() 來進行異步查詢並將結果返回
            return await _context.TPeople.Select(Tp => new CPersonWrap
            {
                FId = Tp.FId,
                FName = Tp.FName,
                FAccount = Tp.FAccount,
                FPassword = Tp.FPassword,
                FGender = Tp.FGender,
                FBirth = Tp.FBirth,
                FPhone = Tp.FPhone,
                FTel = Tp.FTel,
                FAddress = Tp.FAddress,
                FEmail = Tp.FEmail,
                FUbn = Tp.FUbn,
                FPermission = Tp.FPermission,
                FEmp = Tp.FEmp,
                FEmpTel = Tp.FEmpTel,
                FCreatedAt = Tp.FCreatedAt,
                FEditor = Tp.FEditor
            })
        .ToArrayAsync();  // 確保將資料轉換成清單並且異步返回
        }
        //API刪除人員資料
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var person = _context.TPeople.Find(id);
            if (person == null)
            {
                return NotFound();
            }
            _context.TPeople.Remove(person);
            _context.SaveChanges();
            return Ok();
        }


        //詳細資料頁面
        public async Task<IActionResult> Details()        
        {
           return View();
        }
        [HttpGet]        
        public async Task<IActionResult> GetPersonById (int? id) 
        {
            if (id==0) 
            {
                return BadRequest("找不到ID");
            }
            var person = await _context.TPeople.FirstOrDefaultAsync(c => c.FId == id);
            if (person == null)
            {
                return NotFound("Person not found");
            }
            return Ok(person);
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

        //GET: TPersons/Edit/5
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
                //e.FPassword = tPersonwrap.FPassword;
                e.FBirth = tPersonwrap.FBirth;
                e.FPhone = tPersonwrap.FPhone;
                e.FTel = tPersonwrap.FTel;
                e.FAddress = tPersonwrap.FAddress;
                e.FEmail = tPersonwrap.FEmail;
                e.FUbn = tPersonwrap.FUbn;
                e.FPermission = tPersonwrap.FPermission;
                e.FEmp = tPersonwrap.FEmp;
                e.FEmpTel = tPersonwrap.FEmpTel;
                e.FCreatedAt = tPersonwrap.FCreatedAt;
                e.FEditor = tPersonwrap.FEditor;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
        
    }
}
