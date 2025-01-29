using Microsoft.AspNetCore.Mvc;
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
                    FInventoryMainId = detail.FId,
                    FProductId = detail.FProductId,
                    FSystemQuantity = (int)detail.FSystemQuantity,
                    FActualQuantity = detail.FActualQuantity,
                    FName = products.FirstOrDefault(p => p.FId == detail.FProductId)?.FName
                }).ToList();


            // 將 TProduct 轉換為 CProductUpdateWrap
            var productUpdateWraps = products.Select(product => new CProductUpdateWrap
            {
                FId = product.FId,
                FQuantity = product.FQuantity
            }).ToList();

            // 創建 ViewModel 並傳遞到視圖
            var viewModel = new CInventoryViewModel
            {
                InventoryMain = inventoryMainWrap,
                InventoryDetails = inventoryDetailWraps,
                Products = productUpdateWraps // 使用轉換後的 productUpdateWraps
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
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return BadRequest("Invalid model.");
            }
            _logger.LogInformation("Received data: InventoryMain = {InventoryMain}, InventoryDetails = {InventoryDetails}, Products = {Products}",
            viewModel.InventoryMain, viewModel.InventoryDetails, viewModel.Products);
            
            try
            {
                // 記錄 InventoryMain 資料
                _logger.LogInformation("InventoryMain: FBaselineDate = {FBaselineDate}, FCreatedAt = {FCreatedAt}, FEditor = {FEditor}",
                    viewModel.InventoryMain.FBaselineDate, viewModel.InventoryMain.FCreatedAt, viewModel.InventoryMain.FEditor);

                // 記錄 InventoryDetails
                foreach (var detail in viewModel.InventoryDetails)
                {
                    _logger.LogInformation("InventoryDetail: FProductId = {FProductId}, FActualQuantity = {FActualQuantity}, FSystemQuantity = {FSystemQuantity}",
                        detail.FProductId, detail.FActualQuantity, detail.FSystemQuantity);
                }

                // 記錄 Products
                foreach (var product in viewModel.Products)
                {
                    _logger.LogInformation("Product: FId = {FId}, FQuantity = {FQuantity}", product.FId, product.FQuantity);
                }

                // 業務邏輯
                var inventoryMain = _context.TInventoryMains.FirstOrDefault(im => im.FId == currentId);
                if (inventoryMain == null)
                {
                    return NotFound();
                }

                // 更新 TInventoryMain
                inventoryMain.FBaselineDate = viewModel.InventoryMain.FBaselineDate;
                inventoryMain.FCreatedAt = viewModel.InventoryMain.FCreatedAt;
                inventoryMain.FEditor = viewModel.InventoryMain.FEditor;
                inventoryMain.FNote = "自動更新";  // 填入 FNote 預設值

                // 更新 TInventoryDetail
                foreach (var detail in viewModel.InventoryDetails)
                {
                    var inventoryDetail = _context.TInventoryDetails.FirstOrDefault(id => id.FId == detail.FId);
                    if (inventoryDetail != null)
                    {
                        // 確保更新 FSystemQuantity 為 FActualQuantity
                        inventoryDetail.FSystemQuantity = detail.FActualQuantity;

                        // 確保實際庫存被修改
                        _logger.LogInformation("Updating FSystemQuantity for product {FProductId}, new value: {FSystemQuantity}", detail.FProductId, inventoryDetail.FSystemQuantity);
                    }
                    else
                    {
                        // 如果找不到資料，記錄錯誤或警告
                        _logger.LogWarning("InventoryDetail not found for FId: {FId}", detail.FId);
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

                try
                {
                    // 保存所有變更
                    _context.SaveChanges();
                    _logger.LogInformation("Changes saved successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while saving changes to database: {ErrorMessage}", ex.Message);
                    return Json(new { success = false, error = ex.Message });
                }


                return Json(new { success = true, redirectTo = $"/Inventory/Detail/{currentId}" });

            }
            catch (Exception ex)
            {
                // 記錄錯誤訊息
                _logger.LogError("Error occurred while saving inventory data: {ErrorMessage}", ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }
    }

}