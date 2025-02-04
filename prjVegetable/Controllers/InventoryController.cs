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


        /*--------------- + Detail + ----------------*/
        //GET Inventory/Detail
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var inventoryMain = _context.TInventoryMains
                .Where(im => im.FId == id && im.FBaselineDate != null)
                .FirstOrDefault();

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

            // 總 InventoryMain 筆數
            int totalItemCount = _context.TInventoryMains
                .Where(im => im.FBaselineDate != null)
                .Count();

            // 獲取所有有效的 InventoryMain Id 列表
            var validIds = _context.TInventoryMains
                .Where(im => im.FBaselineDate != null)
                .OrderBy(i => i.FId)
                .Select(i => i.FId)
                .ToList();

            // 當前顯示的 InventoryMain Id
            int currentIndex = validIds.IndexOf(id) + 1; 

            var viewModel = new CInventoryViewModel
            {
                InventoryMain = inventoryMainWrap,
                InventoryDetails = inventoryDetailWraps.Any() ? inventoryDetailWraps : new List<CInventoryDetailWrap>(),
                Products = products.Select(product => new CProductUpdateWrap
                {
                    FId = product.FId,
                    FName = product.FName,
                    FQuantity = product.FQuantity
                }).ToList(),
                TotalItemCount = validIds.Count, // 所有有效的 InventoryMain 總數
                CurrentItemCount = currentIndex // 當前顯示的 InventoryMain 是第幾筆
            };

            var firstId = validIds.FirstOrDefault();
            var lastId = validIds.LastOrDefault();
            var previousId = validIds.Where(i => i < id).LastOrDefault(id);
            var nextId = validIds.Where(i => i > id).FirstOrDefault(id);

            ViewData["FirstId"] = firstId;
            ViewData["LastId"] = lastId;
            ViewData["PreviousId"] = previousId;
            ViewData["NextId"] = nextId;

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


        /*---------------- + Save + ------------------*/
        [HttpPost]
        [Route("Inventory/Save/{currentId}")]
        public async Task<IActionResult> Save(int currentId, [FromBody] CInventoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError($"Property: {error.ErrorMessage}");
                }
                _logger.LogError($"Invalid model data: {JsonConvert.SerializeObject(viewModel)}");
                return BadRequest(new { message = "Invalid model data.", details = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

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
                inventoryMain.FNote = "自動更新";

                // 保存 TInventoryMain
                _context.SaveChanges();

                // 更新每個 InventoryDetail 的實際庫存數量
                foreach (var inventoryDetail in viewModel.InventoryDetails)
                {
                    var detailToUpdate = _context.TInventoryDetails
                        .FirstOrDefault(id => id.FId == inventoryDetail.FId);

                    if (detailToUpdate != null)
                    {
                        // 更新實際庫存數量
                        detailToUpdate.FActualQuantity = inventoryDetail.FActualQuantity;
                    }
                }

                // 保存更新的 TInventoryDetail
                _context.SaveChanges();

                // 處理 TInventoryAdjustment
                var inventoryAdjustmentList = new List<TInventoryAdjustment>();
                foreach (var inventoryDetail in viewModel.InventoryDetails)
                {
                    if (inventoryDetail.FActualQuantity != inventoryDetail.FSystemQuantity)
                    {
                        var inventoryAdjustment = new TInventoryAdjustment
                        {
                            FadjustmentDate = inventoryMain.FBaselineDate,
                            FCreatedAt = DateOnly.FromDateTime(DateTime.Now),
                            FEditor = 1,
                            FNote = "庫存調整",
                            FCheckerId = 1
                        };

                        _context.TInventoryAdjustments.Add(inventoryAdjustment);
                        _context.SaveChanges();  // Save to get the Id
                        inventoryAdjustmentList.Add(inventoryAdjustment);
                    }
                }

                // 處理 TInventoryAdjustmentDetail
                foreach (var inventoryDetail in viewModel.InventoryDetails)
                {
                    if (inventoryDetail.FActualQuantity != inventoryDetail.FSystemQuantity)
                    {
                        var linkedAdjustment = inventoryAdjustmentList.FirstOrDefault();
                        if (linkedAdjustment != null)
                        {
                            var goodsDetail = _context.TGoodsInAndOutDetails.FirstOrDefault(g => g.FProductId == inventoryDetail.FProductId);
                            decimal cost = goodsDetail?.FPrice ?? 0;

                            var inventoryAdjustmentDetail = new TInventoryAdjustmentDetail
                            {
                                FInventoryAdjustmentId = linkedAdjustment.FId,
                                FProductId = inventoryDetail.FProductId,
                                FQuantity = (int)(inventoryDetail.FActualQuantity - inventoryDetail.FSystemQuantity),
                                FCost = cost
                            };

                            _context.TInventoryAdjustmentDetails.Add(inventoryAdjustmentDetail);
                        }
                    }
                }

                _context.SaveChanges();

                // 跳轉到新增的 InventoryAdjustment 詳細頁
                var newAdjustmentId = inventoryAdjustmentList.FirstOrDefault()?.FId;
                return Json(new { success = true, redirectTo = $"/InventoryAdjustment/Detail/{newAdjustmentId}" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while saving inventory data: {ErrorMessage}", ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }


        /*---------------- + Search + ----------------*/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(int? fId, DateOnly? fBaselineStartDate, DateOnly? fBaselineEndDate, DateOnly? fCreatedStartDate, DateOnly? fCreatedEndDate)
        {
            var query = _context.TInventoryMains.AsQueryable();

            // 根據 fId 進行篩選
            if (fId.HasValue)
            {
                query = query.Where(x => x.FId == fId.Value);
            }

            // 根據盤點基準日範圍進行篩選
            if (fBaselineStartDate.HasValue)
            {
                query = query.Where(x => x.FBaselineDate >= fBaselineStartDate.Value);
            }

            if (fBaselineEndDate.HasValue)
            {
                query = query.Where(x => x.FBaselineDate <= fBaselineEndDate.Value);
            }

            // 根據建檔日期範圍進行篩選
            if (fCreatedStartDate.HasValue)
            {
                query = query.Where(x => x.FCreatedAt >= fCreatedStartDate.Value);
            }

            if (fCreatedEndDate.HasValue)
            {
                query = query.Where(x => x.FCreatedAt <= fCreatedEndDate.Value);
            }

            // 使用 Join 進行手動聯結
            var inventoryDetails = (from main in query
                                    join detail in _context.TInventoryDetails on main.FId equals detail.FInventoryMainId
                                    select detail)
                                    .ToList();

            // 返回查詢結果
            var result = new
            {
                InventoryDetails = inventoryDetails.Select(item => new
                {
                    item.FProductId,
                    item.FSystemQuantity,
                    item.FActualQuantity
                }).ToList(),
                TotalItemCount = inventoryDetails.Count,
                CurrentItemCount = inventoryDetails.Count
            };

            return Json(result);
        }
    }
}
