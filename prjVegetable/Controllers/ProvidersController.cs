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
    public class ProvidersController : Controller
    {
        private readonly DbVegetableContext _context;

        public ProvidersController(DbVegetableContext context)
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

        //詳細資料頁面
        public async Task<IActionResult> Details()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetProviderById(int? id)
        {
            if (id == 0) 
            {
                return BadRequest("找不到ID");
            }
            var provider = await _context.TProviders.FirstOrDefaultAsync(c => c.FId ==id);
            if (provider == null) 
            { 
                return NotFound("Provider not found");
            }
            return Ok(provider);
        }
        [HttpPut]
        public async Task<IActionResult> update([FromBody] CProviderWrap providerwrap) 
        {
            TProvider e = _context.TProviders.FirstOrDefault(c => c.FId == providerwrap.FId);
            if (e == null) 
            {
                return NotFound("未找到相關廠商");
            }
            try
            { // 更新資料，不給使用者修改的欄位（例如密碼）
                e.FName =providerwrap.FName;
                e.FUbn = providerwrap.FUbn;
                e.FTel = providerwrap.FTel;
                e.FConnect = providerwrap.FConnect;
                e.FAccountant = providerwrap.FAccountant;
                e.FAddress = providerwrap.FAddress;
                e.FDelivery = providerwrap.FDelivery;
                e.FInvoiceAdd = providerwrap.FInvoiceAdd;

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

        //GET: TProviders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var tProvider = await _context.TProviders.FirstOrDefaultAsync(c => c.FId == id);
        //    if (tProvider == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(new CProviderWrap() { provider = tProvider });
        //}

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
                e.FInvoiceAdd = tProviderwrap.FInvoiceAdd;
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
