﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using static prjVegetable.ViewModels.CInventoryViewModel;

namespace prjVegetable.Controllers
{
    public class InventoryController : Controller
    {

        private readonly DbVegetableContext _context;
        private readonly ILogger<InventoryController> _logger;  // 新增 ILogger

        // 修改建構函數，加入 ILogger
        public InventoryController(DbVegetableContext context, ILogger<InventoryController> logger)
        {
            _context = context;
            _logger = logger;  // 注入 ILogger
        }





        public IActionResult Index()
        {
            return View();
        }

        /*--------------- + Detail + ----------------*/

        //GET Inventory/Detail
        public IActionResult Detail(int id)
        {
            // 查詢所有的 InventoryMain
            var inventoryMains = _context.TInventoryMains.OrderBy(m => m.FId).ToList();
            var inventoryDetails = _context.TInventoryDetails.ToList();
            var products = _context.TProducts.ToList();

            // 查詢當前的 inventoryMain
            var inventoryMain = inventoryMains.FirstOrDefault(im => im.FId == id);

            if (inventoryMain == null)
            {
                return NotFound();  // 如果找不到，回傳 404 錯誤
            }

            // 根據傳入的 id 查找相關資料
            var inventoryMainWrap = new CInventoryMainWrap
            {
                FId = inventoryMain.FId,
                FBaselineDate = inventoryMain.FBaselineDate,
                FCreatedAt = inventoryMain.FCreatedAt,
                FEditor = inventoryMain.FEditor,
                FNote = inventoryMain.FNote,
            };

            // 將 TInventoryDetail 轉換為 CInventoryDetailWrap
            var inventoryDetailWraps = inventoryDetails.Where(detail => detail.FInventoryMainId == inventoryMain.FId)
                .Select(detail => new CInventoryDetailWrap
                {
                    FId = detail.FId,
                    FInventoryDetailId = detail.FId,
                    FProductId = detail.FProductId,
                    FSystemQuantity = (int)detail.FSystemQuantity,
                    FActualQuantity = detail.FActualQuantity,
                    FName = products.FirstOrDefault(p => p.FId == detail.FProductId)?.FName
                }).ToList();


            // 將 TProduct 轉換為 CProductWrap
            var productWraps = products.Select(product => new CProductWrap
            {
                FId = product.FId,
                FName = product.FName,
                FQuantity = product.FQuantity,
                FPrice = product.FPrice // 暫時先用此欄位當作成本計算
            }).ToList();

            // 創建 ViewModel 並傳遞到視圖
            var viewModel = new CInventoryViewModel
            {
                InventoryMain = inventoryMainWrap,
                InventoryDetails = inventoryDetailWraps,
                Products = productWraps
            };

            // 查找下一筆和上一筆
            var currentIndex = inventoryMains.FindIndex(im => im.FId == id);

            var nextId = currentIndex < inventoryMains.Count - 1 ? inventoryMains[currentIndex + 1].FId : id;
            var previousId = currentIndex > 0 ? inventoryMains[currentIndex - 1].FId : id;

            // 處理最後一筆的情況，確保 inventoryMains 不為空
            var lastId = inventoryMains.Any() ? inventoryMains.Last().FId : id;

            ViewData["NextId"] = nextId;
            ViewData["PreviousId"] = previousId;
            ViewData["LastId"] = lastId;  // 傳遞最後一筆的 id

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
                FBaselineDate = DateOnly.FromDateTime(BaseDate),
                FCreatedAt = DateOnly.FromDateTime(DateTime.Now),
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
            return RedirectToAction("Detail", "Inventory", new { id = inventoryMain.FId });
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


        /*----------------- + Save + ------------------*/
        [HttpPost]
        [Route("Inventory/Save/{currentId}")]
        public async Task<IActionResult> Save(int currentId, [FromBody] CInventoryViewModel viewModel)
        {
            _logger.LogInformation("Test log entry.");
            try
            {
                var inventoryMain = _context.TInventoryMains.FirstOrDefault(im => im.FId == currentId);
                if (inventoryMain == null)
                {
                    return NotFound();
                }

                // 更新 TInventoryMain
                inventoryMain.FBaselineDate = viewModel.InventoryMain.FBaselineDate;
                inventoryMain.FCreatedAt = viewModel.InventoryMain.FCreatedAt;
                inventoryMain.FEditor = viewModel.InventoryMain.FEditor;

                // 更新 TInventoryDetail
                foreach (var detail in viewModel.InventoryDetails)
                {
                    var inventoryDetail = _context.TInventoryDetails.FirstOrDefault(id => id.FId == detail.FId);
                    if (inventoryDetail != null)
                    {
                        inventoryDetail.FActualQuantity = detail.FActualQuantity;
                    }
                }

                // 更新 TProduct（根據盤點結果更新庫存）
                foreach (var productWrap in viewModel.Products)
                {
                    var inventoryDetail = viewModel.InventoryDetails.FirstOrDefault(id => id.FProductId == productWrap.FId);

                    if (inventoryDetail != null)
                    {
                        productWrap.FQuantity = (int)inventoryDetail.FActualQuantity;
                    }

                    var product = _context.TProducts.FirstOrDefault(p => p.FId == productWrap.FId);
                    if (product != null)
                    {
                        product.FQuantity = productWrap.FQuantity;
                    }
                }

                _context.SaveChanges();

                // 新增一筆到 TInventoryAdjustment
                var inventoryAdjustment = new TInventoryAdjustment
                {
                    FadjustmentDate = DateOnly.FromDateTime(DateTime.Now),
                    FCreatedAt = DateOnly.FromDateTime(DateTime.Now),
                    FEditor = 1, // 假設目前登入的使用者 ID 是 1
                    FNote = "盤點調整記錄"
                };
                _context.TInventoryAdjustments.Add(inventoryAdjustment);
                _context.SaveChanges();

                // 新增對應的 TInventoryAdjustmentDetail
                var adjustmentDetails = viewModel.InventoryDetails.Select(detail => new TInventoryAdjustmentDetail
                {
                    FInventoryAdjustmentId = inventoryAdjustment.FId,
                    FProductId = detail.FProductId,
                    FQuantity = detail.FActualQuantity.HasValue
                        ? detail.FActualQuantity.Value - detail.FSystemQuantity
                        : -detail.FSystemQuantity, // 計算調整數量
                    FCost = viewModel.Products.FirstOrDefault(p => p.FId == detail.FProductId)?.FPrice ?? 0
                }).ToList();

                _context.TInventoryAdjustmentDetails.AddRange(adjustmentDetails);
                _context.SaveChanges();

                // 返回成功信息
                return Json(new { success = true, redirectTo = Url.Action("Detail", "InventoryAdjustment", new { id = inventoryAdjustment.FId }) });
            }
            catch (Exception ex)
            {
                // 處理異常情況
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}