﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using static prjVegetable.ViewModels.CInventoryAdjustmentViewModel;

namespace prjVegetable.Controllers
{
    public class InventoryAdjustmentController : Controller
    {
        public readonly DbVegetableContext _context;

        public InventoryAdjustmentController(DbVegetableContext Context)
        {
            _context = Context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*--------------- + Detail + ----------------*/

        //GET InventoryAdjustment/Detail
        public IActionResult Detail(int? id)
        {
            // 如果沒有提供 id，則自動顯示第一筆調整單
            if (!id.HasValue || id <= 0)
            {
                var firstAdjustment = _context.TInventoryAdjustments.OrderBy(a => a.FId).FirstOrDefault();
                if (firstAdjustment != null)
                {
                    return RedirectToAction("Detail", new { id = firstAdjustment.FId });
                }

                return NotFound("目前沒有任何調整單記錄。");
            }

            // 查詢所有的調整單
            var adjustments = _context.TInventoryAdjustments.OrderBy(a => a.FId).ToList();
            var adjustmentDetails = _context.TInventoryAdjustmentDetails.ToList();
            var products = _context.TProducts.ToList();

            // 查詢當前的調整單
            var adjustment = adjustments.FirstOrDefault(a => a.FId == id);

            if (adjustment == null)
            {
                return NotFound(); // 如果找不到，返回 404
            }

            // 根據傳入的 id 查找相關資料
            var adjustmentWrap = new CInventoryAdjustmentWrap
            {
                FId = adjustment.FId,
                FAdjustmentDate = adjustment.FadjustmentDate,
                FCreatedAt = adjustment.FCreatedAt,
                FEditor = adjustment.FEditor,
                FNote = adjustment.FNote,
                FCheckerId = adjustment.FCheckerId
            };

            // 將 TInventoryAdjustmentDetail 轉換為 CInventoryAdjustmentDetailWrap
            var adjustmentDetailWraps = adjustmentDetails.Where(detail => detail.FInventoryAdjustmentId == adjustment.FId)
                .Select(detail => new CInventoryAdjustmentDetailWrap
                {
                    FId = detail.FId,
                    FProductId = detail.FProductId,
                    FQuantity = (int)detail.FQuantity,
                    FName = products.FirstOrDefault(p => p.FId == detail.FProductId)?.FName,
                    FCost = products.FirstOrDefault(p => p.FId == detail.FProductId)?.FPrice ?? 0
                }).ToList();

            // 將 TProduct 轉換為 CProductWrap
            var productWraps = products.Select(product => new CProductWrap
            {
                FId = product.FId,
                FName = product.FName,
                FQuantity = product.FQuantity,
                FPrice = product.FPrice
            }).ToList();

            // 創建 ViewModel 並傳遞到視圖
            var viewModel = new CInventoryAdjustmentViewModel
            {
                InventoryAdjustment = adjustmentWrap,
                InventoryAdjustmentDetail = adjustmentDetailWraps,
                Products = productWraps
            };

            // 查找下一筆和上一筆
            var currentIndex = adjustments.FindIndex(a => a.FId == id);

            var nextId = currentIndex < adjustments.Count - 1 ? adjustments[currentIndex + 1].FId : id;
            var previousId = currentIndex > 0 ? adjustments[currentIndex - 1].FId : id;

            // 處理最後一筆的情況，確保 adjustments 不為空
            var lastId = adjustments.Any() ? adjustments.Last().FId : id;

            ViewData["NextId"] = nextId;
            ViewData["PreviousId"] = previousId;
            ViewData["LastId"] = lastId; // 傳遞最後一筆的 id

            // 返回視圖
            return View(viewModel);
        }


        /*--------------- + Create + ----------------*/
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DateTime BaseDate, int ProductStartCode, int ProductEndCode)
        {
            var inventoryMain = new TInventoryMain
            {
                FBaselineDate = DateTime.Now, // 直接使用 DateTime 來指定日期
                FCreatedAt = DateTime.Now,    // 使用當前時間
                FEditor = 1,
                FNote = "新增盤點條件"
            };

            _context.TInventoryMains.Add(inventoryMain);
            _context.SaveChanges();

            var inventoryDetails = new List<TInventoryDetail>();

            var productsInRange = _context.TProducts
                .Where(p => p.FId >= ProductStartCode && p.FId <= ProductEndCode)
                .ToList();

            foreach (var product in productsInRange)
            {
                var inventoryDetail = new TInventoryDetail
                {
                    FInventoryMainId = inventoryMain.FId,
                    FProductId = product.FId,
                    FSystemQuantity = product.FQuantity,
                    FActualQuantity = null
                };

                inventoryDetails.Add(inventoryDetail);
            }

            _context.TInventoryDetails.AddRange(inventoryDetails);
            _context.SaveChanges();

            // 使用創建的 inventoryMain.FId 作為重定向的 ID
            return RedirectToAction("Detail", "InventoryAdjustment", new { id = inventoryMain.FId });
        }



        /*--------------- + Delete + ----------------*/
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // 根據 ID 查找要刪除的盤點主單
            var inventoryMain = _context.TInventoryMains.FirstOrDefault(im => im.FId == id);
            if (inventoryMain == null)
            {
                return NotFound();  // 找不到資料則返回 404 錯誤
            }

            // 創建 ViewModel 並傳遞資料至視圖
            return View(inventoryMain);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                // 查找要刪除的盤點主單
                var inventoryMain = _context.TInventoryMains.FirstOrDefault(im => im.FId == id);
                if (inventoryMain == null)
                {
                    return NotFound();
                }

                // 刪除相關的 TInventoryDetail 資料
                var inventoryDetails = _context.TInventoryDetails.Where(detail => detail.FId == id).ToList();
                _context.TInventoryDetails.RemoveRange(inventoryDetails);
                _context.TInventoryMains.Remove(inventoryMain);

                // 提交變更
                _context.SaveChanges();

                // 查找前一筆資料（ID 小於當前資料的最大值）
                var previousInventoryMain = _context.TInventoryMains
                    .Where(im => im.FId < id)
                    .OrderByDescending(im => im.FId)  // 降序排列
                    .FirstOrDefault();  // 取得最新的一筆

                // 返回一個包含重定向 URL 的 JSON 響應
                return Json(new { success = true, redirectTo = Url.Action("Detail", new { id = previousInventoryMain?.FId }) });
            }
            catch (Exception ex)
            {
                // 發生錯誤，返回失敗狀態
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}
