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
            // 查詢所有有效的 InventoryMain ID 列表
            var validIds = _context.TInventoryMains
                .Where(im => im.FBaselineDate != null)
                .OrderBy(i => i.FId)
                .Select(i => i.FId)
                .ToList();

            // 如果沒有提供 ID，或者 ID 無效，則改為使用最新的一筆有效資料
            if (id == null || !validIds.Contains(id.Value))
            {
                id = validIds.LastOrDefault(); // 取最後一筆（最新的）
            }

            // 查詢 InventoryMain
            var inventoryMain = _context.TInventoryMains
                .FirstOrDefault(im => im.FId == id && im.FBaselineDate != null);

            // 如果找不到 InventoryMain，則建立空的 ViewModel
            var inventoryMainWrap = inventoryMain != null
                ? new CInventoryMainWrap
                {
                    FId = inventoryMain.FId,
                    FBaselineDate = inventoryMain.FBaselineDate,
                    FCreatedAt = inventoryMain.FCreatedAt,
                    FEditor = inventoryMain.FEditor,
                    FNote = inventoryMain.FNote,
                }
                : new CInventoryMainWrap(); // 空物件

            // 查詢所有產品
            var products = _context.TProducts.Select(product => new CProductUpdateWrap
            {
                FId = product.FId,
                FName = product.FName,
                FQuantity = product.FQuantity
            }).ToList();

            var productDict = _context.TProducts
            .ToDictionary(p => p.FId, p => p.FName); // 先轉成 Dictionary

            var inventoryDetails = inventoryMain != null
                ? _context.TInventoryDetails
                    .Where(detail => detail.FInventoryMainId == inventoryMain.FId)
                    .ToList() // 先執行 SQL 查詢，轉換為記憶體內的物件
                    .Select(detail => new CInventoryDetailWrap
                    {
                        FId = detail.FId,
                        FInventoryMainId = detail.FInventoryMainId,
                        FProductId = detail.FProductId,
                        FSystemQuantity = (int)detail.FSystemQuantity,
                        FActualQuantity = detail.FActualQuantity,
                        FName = productDict.ContainsKey(detail.FProductId) ? productDict[detail.FProductId] : null // 避免 EF 查詢錯誤
                    }).ToList()
                : new List<CInventoryDetailWrap>();

            // 查詢所有具備權限的員工
            var employees = _context.TPeople
                .Where(p => p.FPermission == 1)
                .ToList();
            ViewData["Employees"] = new SelectList(employees, "FId", "FName");

            // 計算當前 ID 在 validIds 裡的索引
            int totalItemCount = validIds.Count;
            int currentIndex = id.HasValue ? validIds.IndexOf(id.Value) + 1 : 0;

            // 計算前一筆、下一筆、第一筆和最後一筆的 ID
            var firstId = validIds.FirstOrDefault();
            var lastId = validIds.LastOrDefault();
            var previousId = validIds.Where(i => i < id).LastOrDefault(firstId);
            var nextId = validIds.Where(i => i > id).FirstOrDefault(lastId);

            // 建立 ViewModel
            var viewModel = new CInventoryViewModel
            {
                InventoryMain = inventoryMainWrap,
                InventoryDetails = inventoryDetails,
                Products = products,
                TotalItemCount = totalItemCount,
                CurrentItemCount = currentIndex
            };

            // 設定 ViewData 用於前端導覽按鈕
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
            // 查詢範圍內的商品
            var productsInRange = _context.TProducts
                .Where(p => p.FId >= ProductStartCode && p.FId <= ProductEndCode)
                .OrderBy(p => p.FId) // 按商品ID排序，確保範圍描述一致
                .ToList();

            // 確定範圍的首尾商品
            var firstProduct = productsInRange.FirstOrDefault();
            var lastProduct = productsInRange.LastOrDefault();

            // 如果範圍內沒有產品，回傳錯誤訊息
            if (firstProduct == null || lastProduct == null)
            {
                return BadRequest("指定的商品範圍內沒有任何產品！");
            }

            // 商品範圍描述
            var productRangeNote = $"{firstProduct.FId}-{firstProduct.FName} ~ {lastProduct.FId}-{lastProduct.FName}";

            // 建立庫存主檔
            var inventoryMain = new TInventoryMain
            {
                FBaselineDate = BaseDate,
                FCreatedAt = DateOnly.FromDateTime(DateTime.Now),
                FEditor = 1,
                FNote = $"新增庫存盤點單 - 商品範圍: {productRangeNote}" // 將商品範圍加入備註
            };

            _context.TInventoryMains.Add(inventoryMain);
            _context.SaveChanges();

            var inventoryDetails = new List<TInventoryDetail>();

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

            // 導向到明細頁
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
