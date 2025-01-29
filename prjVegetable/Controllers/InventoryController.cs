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
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(DbVegetableContext context, ILogger<InventoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*--------------- + Detail + ----------------*/
        //GET Inventory/Detail
        public IActionResult Detail(int id)
        {
            // 直接查詢符合 id 的盤點資料
            var inventoryMain = _context.TInventoryMains
                .FirstOrDefault(im => im.FId == id);

            // 如果找不到對應的盤點資料，返回空資料頁面
            if (inventoryMain == null)
            {
                return View("DetailEmpty", new CInventoryViewModel
                {
                    InventoryMain = new CInventoryMainWrap(),
                    InventoryDetails = new List<CInventoryDetailWrap>(),
                    Products = _context.TProducts.Select(product => new CProductUpdateWrap
                    {
                        FId = product.FId,
                        FQuantity = product.FQuantity
                    }).ToList()
                });
            }

            var inventoryMainWrap = new CInventoryMainWrap
            {
                FId = inventoryMain.FId,
                FBaselineDate = inventoryMain.FBaselineDate,
                FCreatedAt = inventoryMain.FCreatedAt,
                FEditor = inventoryMain.FEditor,
                FNote = inventoryMain.FNote,
            };

            var inventoryDetails = _context.TInventoryDetails
                .Where(detail => detail.FInventoryMainId == inventoryMain.FId)
                .ToList();

            var products = _context.TProducts.ToList();

            var inventoryDetailWraps = inventoryDetails.Select(detail => new CInventoryDetailWrap
            {
                FId = detail.FId,
                FInventoryMainId = detail.FInventoryMainId,
                FProductId = detail.FProductId,
                FSystemQuantity = (int)detail.FSystemQuantity,
                FActualQuantity = detail.FActualQuantity,
                FName = products.FirstOrDefault(p => p.FId == detail.FProductId)?.FName
            }).ToList();

            var viewModel = new CInventoryViewModel
            {
                InventoryMain = inventoryMainWrap,
                InventoryDetails = inventoryDetailWraps.Any() ? inventoryDetailWraps : new List<CInventoryDetailWrap>(),
                Products = products.Select(product => new CProductUpdateWrap
                {
                    FId = product.FId,
                    FQuantity = product.FQuantity
                }).ToList()
            };

            // 計算下一個、上一個以及最後一個 id
            var inventoryMains = _context.TInventoryMains.ToList();
            var currentIndex = inventoryMains.FindIndex(im => im.FId == id);
            var nextId = currentIndex < inventoryMains.Count - 1 ? inventoryMains[currentIndex + 1].FId : id;
            var previousId = currentIndex > 0 ? inventoryMains[currentIndex - 1].FId : id;
            var lastId = inventoryMains.Any() ? inventoryMains.Last().FId : id;

            ViewData["NextId"] = nextId;
            ViewData["PreviousId"] = previousId;
            ViewData["LastId"] = lastId;

            return View(viewModel);
        }


        /*--------------- + Create + ----------------*/
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DateOnly BaseDate, int ProductStartCode, int ProductEndCode)
        {
            var inventoryMain = new TInventoryMain
            {
                FBaselineDate = BaseDate,
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
                return NotFound();
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
                    .OrderByDescending(im => im.FId)
                    .FirstOrDefault();

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
                inventoryMain.FNote = "自動更新";

                // 更新 TInventoryDetail
                foreach (var detail in viewModel.InventoryDetails)
                {
                    var inventoryDetail = _context.TInventoryDetails.FirstOrDefault(id => id.FId == detail.FId);
                    if (inventoryDetail != null)
                    {
                        inventoryDetail.FSystemQuantity = detail.FActualQuantity;
                        _logger.LogInformation("Updating FSystemQuantity for product {FProductId}, new value: {FSystemQuantity}", detail.FProductId, inventoryDetail.FSystemQuantity);
                    }
                    else
                    {
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
                _logger.LogError("Error occurred while saving inventory data: {ErrorMessage}", ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }
    }

}