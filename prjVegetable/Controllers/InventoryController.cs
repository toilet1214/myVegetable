using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using static prjVegetable.ViewModels.CInventoryViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [HttpGet]
        public IActionResult Detail(int? id)
        {
            // 如果沒有提供 ID 或查不到該 ID，導向最新一筆有效資料
            if (id == null || !_context.TInventoryMains.Any(im => im.FId == id && im.FBaselineDate != null))
            {
                // 查詢最新一筆有效資料的 ID（以 FId 降序排列）
                var latestRecordId = _context.TInventoryMains
                    .Where(im => im.FBaselineDate != null)
                    .OrderByDescending(im => im.FId)
                    .Select(im => im.FId)
                    .FirstOrDefault();

                if (latestRecordId != 0) // 如果找到最新一筆資料，就導向該筆資料
                {
                    return RedirectToAction("Detail", new { id = latestRecordId });
                }
                else // 如果沒有任何有效資料，則顯示空視圖
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
            }

            // 查詢對應的 InventoryMain 資料
            var inventoryMain = _context.TInventoryMains
                .Where(im => im.FId == id && im.FBaselineDate != null)
                .FirstOrDefault();

            // 包裝 InventoryMain 資料
            var inventoryMainWrap = new CInventoryMainWrap
            {
                FId = inventoryMain.FId,
                FBaselineDate = inventoryMain.FBaselineDate,
                FCreatedAt = inventoryMain.FCreatedAt,
                FEditor = inventoryMain.FEditor,
                FNote = inventoryMain.FNote,
            };

            // 查詢相關的 InventoryDetails
            var inventoryDetails = _context.TInventoryDetails
                .Where(detail => detail.FInventoryMainId == inventoryMain.FId)
                .ToList();

            // 查詢產品資料
            var products = _context.TProducts.ToList();

            // 包裝 InventoryDetails
            var inventoryDetailWraps = inventoryDetails.Select(detail => new CInventoryDetailWrap
            {
                FId = detail.FId,
                FInventoryMainId = detail.FInventoryMainId,
                FProductId = detail.FProductId,
                FSystemQuantity = (int)detail.FSystemQuantity,
                FActualQuantity = detail.FActualQuantity,
                FName = products.FirstOrDefault(p => p.FId == detail.FProductId)?.FName
            }).ToList();

            // 查詢員工資料（只包含具備特定權限的）
            var employees = _context.TPeople
                .Where(p => p.FPermission == 1)
                .ToList();

            // 使用 SelectList 封裝員工資料
            ViewData["Employees"] = new SelectList(employees, "FId", "FName");

            // 獲取有效的 InventoryMain Id 列表
            var validIds = _context.TInventoryMains
                .Where(im => im.FBaselineDate != null)
                .OrderBy(i => i.FId)
                .Select(i => i.FId)
                .ToList();

            // 獲取當前顯示的資料索引與總數
            int totalItemCount = validIds.Count;
            int currentIndex = validIds.IndexOf(id.Value) + 1;

            // 計算前一筆、下一筆、第一筆和最後一筆的 ID
            var firstId = validIds.Cast<int?>().FirstOrDefault() ?? 0;
            var lastId = validIds.Cast<int?>().LastOrDefault() ?? 0;
            var previousId = validIds.Cast<int?>().Where(i => i < id).LastOrDefault(id.Value);
            var nextId = validIds.Cast<int?>().Where(i => i > id).FirstOrDefault(id.Value);

            // 構建 ViewModel
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
                TotalItemCount = totalItemCount, // 所有有效的 InventoryMain 總數
                CurrentItemCount = currentIndex // 當前顯示的 InventoryMain 是第幾筆
            };

            ViewData["FirstId"] = firstId;
            ViewData["LastId"] = lastId;
            ViewData["PreviousId"] = previousId;
            ViewData["NextId"] = validIds.Cast<int?>().Where(i => i > id).FirstOrDefault() ?? lastId;



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
        public async Task<IActionResult> Save(int currentId, [FromBody] CInventoryViewModel viewModel, [FromQuery] bool redirect = true)
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

                // 取得原始 FActualQuantity
                var originalQuantities = _context.TInventoryDetails
                    .Where(d => viewModel.InventoryDetails.Select(vd => vd.FId).Contains(d.FId))
                    .ToDictionary(d => d.FId, d => d.FActualQuantity);

                // 更新 TInventoryMain
                inventoryMain.FBaselineDate = viewModel.InventoryMain.FBaselineDate;
                inventoryMain.FCreatedAt = viewModel.InventoryMain.FCreatedAt;
                inventoryMain.FEditor = viewModel.InventoryMain.FEditor;
                inventoryMain.FNote = viewModel.InventoryMain.FNote;
                _context.SaveChanges();

                // 更新 TInventoryDetail
                bool hasQuantityChange = false;
                foreach (var inventoryDetail in viewModel.InventoryDetails)
                {
                    var detailToUpdate = _context.TInventoryDetails
                        .FirstOrDefault(id => id.FId == inventoryDetail.FId);

                    if (detailToUpdate != null)
                    {
                        if (originalQuantities.ContainsKey(detailToUpdate.FId) &&
                            originalQuantities[detailToUpdate.FId] != inventoryDetail.FActualQuantity)
                        {
                            hasQuantityChange = true;
                        }

                        detailToUpdate.FActualQuantity = inventoryDetail.FActualQuantity;
                    }
                }
                _context.SaveChanges();

                // 更新 TProduct 的 FQuantity
                foreach (var product in viewModel.Products)
                {
                    var productToUpdate = _context.TProducts.FirstOrDefault(p => p.FId == product.FId);
                    if (productToUpdate != null)
                    {
                        productToUpdate.FQuantity = product.FQuantity;
                    }
                }
                _context.SaveChanges();

                // 若數量沒有變更，直接返回成功訊息
                if (!hasQuantityChange)
                {
                    return Json(new { success = true });
                }

                // 處理 TInventoryAdjustment
                var inventoryAdjustmentList = new List<TInventoryAdjustment>();
                foreach (var inventoryDetail in viewModel.InventoryDetails)
                {
                    if (originalQuantities[inventoryDetail.FId] != inventoryDetail.FActualQuantity)
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
                        _context.SaveChanges();
                        inventoryAdjustmentList.Add(inventoryAdjustment);
                    }
                }

                // 處理 TInventoryAdjustmentDetail
                foreach (var inventoryDetail in viewModel.InventoryDetails)
                {
                    if (originalQuantities[inventoryDetail.FId] != inventoryDetail.FActualQuantity)
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
                if (redirect)
                {
                    var newAdjustmentId = inventoryAdjustmentList.FirstOrDefault()?.FId;
                    return Json(new
                    {
                        success = true,
                        redirectTo = newAdjustmentId.HasValue
                            ? $"/InventoryAdjustment/Detail/{newAdjustmentId}"
                            : $"/Inventory/Detail/{currentId}"
                    });
                }

                // 如果不跳轉，直接返回成功訊息
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while saving inventory data: {ErrorMessage}", ex.Message);
                return Json(new { success = false, error = ex.Message });
            }
        }



        /*---------------- + Search + ----------------*/
        public IActionResult Search(int? fId, DateOnly? fBaselineStartDate, DateOnly? fBaselineEndDate, DateOnly? fCreatedStartDate, DateOnly? fCreatedEndDate)
        {
            var query = _context.TInventoryMains.AsQueryable();

            // 1. 根據 FId 查詢 (允許其他條件並存)
            if (fId.HasValue)
            {
                query = query.Where(i => i.FId == fId.Value);
            }

            // 2. 日期範圍過濾
            if (fBaselineStartDate.HasValue && fBaselineEndDate.HasValue && fBaselineStartDate > fBaselineEndDate)
            {
                return Json(new { success = false, message = "盤點基準日範圍不正確" });
            }

            if (fCreatedStartDate.HasValue && fCreatedEndDate.HasValue && fCreatedStartDate > fCreatedEndDate)
            {
                return Json(new { success = false, message = "建檔日期範圍不正確" });
            }

            if (fBaselineStartDate.HasValue)
            {
                query = query.Where(i => i.FBaselineDate >= fBaselineStartDate.Value);
            }

            if (fBaselineEndDate.HasValue)
            {
                query = query.Where(i => i.FBaselineDate <= fBaselineEndDate.Value);
            }

            if (fCreatedStartDate.HasValue)
            {
                query = query.Where(i => i.FCreatedAt >= fCreatedStartDate.Value);
            }

            if (fCreatedEndDate.HasValue)
            {
                query = query.Where(i => i.FCreatedAt <= fCreatedEndDate.Value);
            }

            // 3. 查詢主檔並加上 FNote 欄位
            var inventoryMains = query.Select(i => new CInventoryMainWrap
            {
                FId = i.FId,
                FBaselineDate = i.FBaselineDate,
                FCreatedAt = i.FCreatedAt,
                FNote = i.FNote // 加入 FNote 欄位
            }).ToList();

            if (!inventoryMains.Any())
            {
                return Json(new { success = false, message = "未找到符合條件的盤點主資料。" });
            }

            _logger.LogInformation("查詢到的 InventoryMain: {0}", JsonConvert.SerializeObject(inventoryMains));

            // 4. 回傳符合條件的 InventoryMain 清單
            return Json(new
            {
                success = true,
                TotalCount = inventoryMains.Count,
                InventoryMainList = inventoryMains
            });
        }
    }
}
