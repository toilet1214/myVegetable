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
            // 使用 DateOnly.Today 來設置 FCreatedAt 為今天的日期
            tPersonwrap.person.FCreatedAt = DateTime.Now;  // 設定為今天的日期，去除時間部分

            // 設定 FEditor 為當前使用者的 ID
            //if (int.TryParse(User.Identity.Name, out int editorId))
            //{
            //    tPersonwrap.person.FEditor = editorId;//// 設置為當前使用者的數字 ID
            //}
            //else 
            //{
            //    // 如果轉換失敗，可以設置為 0 或處理錯誤邏輯
            //    tPersonwrap.person.FEditor = 0;
            //}


            // 將新的 TPerson 物件加入到資料庫上下文
            _context.TPeople.Add(tPersonwrap.person);

            // 儲存變更
            await _context.SaveChangesAsync();

            // 重定向回 Index 頁面
            return RedirectToAction(nameof(Index));
        }

        // GET: TPersons/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var x = await _context.TPeople.FirstOrDefaultAsync(c => c.FId == id);
        //    if (x == null)
        //        return RedirectToAction(nameof(Index));

        //    return View(new CPersonWrap() { person = x });
        //}

        // POST: TPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> update([FromBody] CPersonWrap personwrap)
        {
            // 檢查是否找到對應的使用者
            TPerson e = _context.TPeople.FirstOrDefault(c => c.FId == personwrap.FId);
            Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int UserId);
            if (e == null)
            {
                return NotFound("使用者未找到");
            }

            var existingAccount = await _context.TPeople.FirstOrDefaultAsync(p => p.FAccount == personwrap.FAccount && p.FId != personwrap.FId);

            if (existingAccount!=null) 
            {
                return BadRequest("此帳號已存在，請選擇其他帳號");
            }

            try
            {
                // 更新資料，不給使用者修改的欄位（例如密碼）
                e.FName = personwrap.FName;
                //e.FAccount = personwrap.FAccount;
                e.FBirth = personwrap.FBirth;
                e.FPhone = personwrap.FPhone;
                e.FTel = personwrap.FTel;
                e.FAddress = personwrap.FAddress;
                e.FEmail = personwrap.FEmail;
                e.FUbn = personwrap.FUbn;
                e.FPermission = personwrap.FPermission;
                e.FEmp = personwrap.FEmp;
                e.FEmpTel = personwrap.FEmpTel;                
                e.FEditor = UserId;
                e.FPassword = personwrap.FPassword;

                // 儲存變更
                await _context.SaveChangesAsync();

                return Ok("資料已成功更新");
            }
            catch (Exception ex)
            {
                // 捕捉錯誤並回傳具體錯誤訊息
                return StatusCode(500, $"儲存資料時發生錯誤: {ex.Message}");
            }
        }

    }
}
