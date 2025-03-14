﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using static prjVegetable.ViewModels.CInventoryAdjustmentViewModel;

namespace prjVegetable.Controllers
{
    public class InventoryAdjustmentController : Controller
    {
        private readonly DbVegetableContext _context;
        private readonly ILogger<InventoryAdjustmentController> _logger;

        public InventoryAdjustmentController(DbVegetableContext context, ILogger<InventoryAdjustmentController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        /*--------------- + Detail + ----------------*/
        [HttpGet]
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
                return NotFound();
            }

            // 將 TInventoryAdjustment 轉換為 CInventoryAdjustmentWrap
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

            // 取得盤點人員資料
            var employees = _context.TPeople
                .Where(p => p.FPermission == 1)
                .ToList();

            var checkerId = adjustment.FCheckerId;
            var checker = _context.TPeople
                .FirstOrDefault(p => p.FId == checkerId);
            var checkerName = checker?.FName ?? "";
            
            
            // 傳遞盤點人員的名稱
            ViewData["CheckerName"] = checkerName;

            // 使用 SelectList 封裝員工資料
            ViewData["Employees"] = new SelectList(employees, "FId", "FName");
            
            // 計算當前記錄的索引及上一筆、下一筆資料的 Id
            var currentIndex = adjustments.FindIndex(a => a.FId == id);
            if (currentIndex == -1)
            {
                _logger.LogError($"無法找到對應的記錄，FId: {id}");
                return NotFound(new { message = "找不到對應的記錄。" });
            }

            int totalItemCount = adjustments.Count;
            int currentItemCount = currentIndex + 1;

            // 創建 ViewModel 並傳遞到視圖
            var viewModel = new CInventoryAdjustmentViewModel
            {
                InventoryAdjustment = adjustmentWrap,
                InventoryAdjustmentDetail = adjustmentDetailWraps,
                Products = productWraps.Select(p => new CProductUpdateWrap
                {
                    FId = p.FId,
                    FQuantity = p.FQuantity,
                    FName = p.FName
                }).ToList(),
                TotalItemCount = totalItemCount,
                CurrentItemCount = currentItemCount
            };

            // 動態取得第一筆與最後一筆的 FId
            var firstId = adjustments.Any() ? adjustments.First().FId : id;
            var lastId = adjustments.Any() ? adjustments.Last().FId : id;


            var nextId = currentIndex < adjustments.Count - 1 ? adjustments[currentIndex + 1].FId : -1;
            var previousId = currentIndex > 0 ? adjustments[currentIndex - 1].FId : -1;

            // 判斷是否為第一筆或最後一筆
            var isFirstRecord = currentIndex == 0;
            var isLastRecord = currentIndex == adjustments.Count - 1;

            // 將資料傳遞到 View
            ViewData["PreviousId"] = previousId > 0 ? previousId : null;
            ViewData["NextId"] = nextId > 0 ? nextId : null;
            ViewData["FirstId"] = firstId;
            ViewData["LastId"] = lastId;
            ViewData["IsFirstRecord"] = isFirstRecord;
            ViewData["IsLastRecord"] = isLastRecord;

            return View(viewModel);

        }


        /*--------------- + Delete + ----------------*/
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var inventoryAdjustment = _context.TInventoryAdjustments.FirstOrDefault(im => im.FId == id);
            if (inventoryAdjustment == null)
            {
                return NotFound();
            }

            return View(inventoryAdjustment);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                // 查找要刪除的盤點主單
                var inventoryAdjustment = _context.TInventoryAdjustments.FirstOrDefault(im => im.FId == id);
                if (inventoryAdjustment == null)
                {
                    return NotFound();
                }

                // 刪除相關的 TInventoryDetail 資料
                var inventoryAdjustmentsDetails = _context.TInventoryAdjustmentDetails.Where(detail => detail.FId == id).ToList();
                _context.TInventoryAdjustmentDetails.RemoveRange(inventoryAdjustmentsDetails);
                _context.TInventoryAdjustments.Remove(inventoryAdjustment);

                // 提交變更
                _context.SaveChanges();

                // 查找前一筆資料（ID 小於當前資料的最大值）
                var previousInventoryAdjustments = _context.TInventoryAdjustments
                    .Where(im => im.FId < id)
                    .OrderByDescending(im => im.FId)  // 降序排列
                    .FirstOrDefault();  // 取得最新的一筆

                // 返回一個包含重定向 URL 的 JSON 響應
                return Json(new { success = true, redirectTo = Url.Action("Detail", new { id = previousInventoryAdjustments?.FId }) });
            }
            catch (Exception ex)
            {
                // 發生錯誤，返回失敗狀態
                return Json(new { success = false, error = ex.Message });
            }
        }


        /*---------------- + Save + ------------------*/
        [HttpPost]
        [Route("InventoryAdjustment/Save/{currentId}")]
        public async Task<IActionResult> Save(int currentId, [FromBody] IEnumerable<CInventoryAdjustmentUpdateWrap> adjustmentUpdate)
        {
            if (adjustmentUpdate == null || !adjustmentUpdate.Any())
            {
                _logger.LogError("No adjustment data provided.");
                return BadRequest(new { message = "No adjustment data provided." });
            }

            try
            {
                var inventoryAdjustment = _context.TInventoryAdjustments.FirstOrDefault(ia => ia.FId == currentId);
                if (inventoryAdjustment == null)
                {
                    return NotFound(new { message = "Inventory Adjustment record not found." });
                }

                // 只處理 AdjustmentUpdate 資料
                var updateData = adjustmentUpdate.FirstOrDefault();
                if (updateData != null && updateData.FId == currentId)
                {
                    inventoryAdjustment.FNote = updateData.FNote;
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while saving InventoryAdjustment FNote: {ErrorMessage}", ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }

        /*---------------- + Search + ----------------*/
        [HttpGet]
        public IActionResult Search(int? fId, DateOnly? fBaselineStartDate, DateOnly? fBaselineEndDate, int? fCheckerId)
        {
            try
            {
                var query = _context.TInventoryAdjustments.AsQueryable();

                // 1. 根據 FId 過濾
                if (fId.HasValue)
                {
                    query = query.Where(i => i.FId == fId.Value);
                }

                // 2. 盤點基準日範圍過濾
                if (fBaselineStartDate.HasValue && fBaselineEndDate.HasValue && fBaselineStartDate > fBaselineEndDate)
                {
                    return Json(new { success = false, message = "盤點基準日範圍不正確" });
                }

                if (fBaselineStartDate.HasValue)
                {
                    query = query.Where(i => i.FadjustmentDate >= fBaselineStartDate.Value);
                }

                if (fBaselineEndDate.HasValue)
                {
                    query = query.Where(i => i.FadjustmentDate <= fBaselineEndDate.Value);
                }

                // 3. 根據盤點人員 FCheckerId 過濾
                if (fCheckerId.HasValue)
                {
                    query = query.Where(i => i.FCheckerId == fCheckerId.Value);
                }

                // 4. 查詢結果，新增 FNote
                var inventoryAdjustmentList = (from i in query
                                               join p in _context.TPeople
                                               on i.FCheckerId equals p.FId into personGroup
                                               from person in personGroup.DefaultIfEmpty()
                                               select new
                                               {
                                                   i.FId,
                                                   FAdjustmentDate = i.FadjustmentDate,
                                                   FCheckerName = person != null ? person.FName : "未指定",
                                                   FNote = i.FNote ?? "無備註", // 新增 FNote
                                                   i.FCreatedAt
                                               }).ToList();

                // 5. 檢查是否有資料
                if (!inventoryAdjustmentList.Any())
                {
                    return Json(new { success = false, message = "查無符合條件的資料。" });
                }

                // 6. 回應查詢結果
                return Json(new
                {
                    success = true,
                    inventoryAdjustmentList = inventoryAdjustmentList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("查詢失敗：{Message}", ex.Message);
                return Json(new { success = false, message = "查詢過程中發生錯誤，請稍後再試。" });
            }
        }



    }
}
