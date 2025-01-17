using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using static prjVegetable.ViewModels.CInventoryViewModel;

namespace prjVegetable.Controllers
{
    public class InventoryController : Controller
    {
        public readonly DbVegetableContext _context;

        public InventoryController(DbVegetableContext Context)
        {
            _context = Context;
        }

        public IActionResult Index()
        {
            return View();
        }

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
            var inventoryDetailWraps = inventoryDetails.Where(detail => detail.FInventoryDetailId == inventoryMain.FId)
                .Select(detail => new CInventoryDetailWrap
                {
                    FId = detail.FId,
                    FInventoryDetailId = detail.FInventoryDetailId,
                    FProductId = detail.FProductId,
                    FSystemQuantity = detail.FSystemQuantity,
                    FActualQuantity = detail.FActualQuantity,
                    FName = products.FirstOrDefault(p => p.FId == detail.FProductId)?.FName
                }).ToList();

            // 將 TProduct 轉換為 CProductWrap
            var productWraps = products.Select(product => new CProductWrap
            {
                FId = product.FId,
                FName = product.FName,
                FQuantity = product.FQuantity,
                FLaunchAt = product.FLaunchAt,
                FOrigin = product.FOrigin
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
                    FInventoryDetailId = inventoryMain.FId,
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


        // 更新實際庫存數量

    }
}
























    /*------------ + 排版參考用頁面 + -------------*/
    //GET Inventory/InventoryAdjustment
    //public IActionResult InventoryAdjustment()
    //{
    //    var inventoryMain = _context.TInventoryMains.FirstOrDefault();
    //    var inventoryDetails = _context.TInventoryDetails.ToList();
    //    var inventoryMainWrap = new CInventoryMainWrap
    //    {
    //        FId = inventoryMain.FId,
    //        FInventoryNo = inventoryMain.FInventoryNo,
    //        FBaselineDate = inventoryMain.FBaselineDate,
    //        FCreatedAt = inventoryMain.FCreatedAt,
    //        FCreatorId = inventoryMain.FCreatorId,
    //        FEditorId = inventoryMain.FEditorId
    //    };
    //    var inventoryDetailWraps = inventoryDetails.Select(detail => new CInventoryDetailWrap
    //    {
    //        FId = detail.FId,
    //        FInventoryNo = detail.FInventoryNo,
    //        FProductId = detail.FProductId,
    //        FProductName = detail.FProductName,
    //        FSystemQuantity = detail.FSystemQuantity,
    //        FActualQuantity = detail.FActualQuantity,
    //        FDifferenceQuantity = detail.FDifferenceQuantity,
    //        FAmount = detail.FAmount,
    //        FRemark = detail.FRemark,
    //        FEditorId = detail.FEditorId
    //    }).ToList();
    //    var viewModel = new CInventoryViewModel
    //    {
    //        InventoryMain = inventoryMainWrap,
    //        InventoryDetails = inventoryDetailWraps
    //    };
    //    return View(viewModel);
    //}

