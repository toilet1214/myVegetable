using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;

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
        public IActionResult Detail()
        {
            /*
                為什麼需要將資料庫模型 TInventoryMain 和 TInventoryDetail
                轉換為在視圖中使用的 CInventoryMainWrap 和 CInventoryDetailWrap ?
                
                1. 可以選擇只將必要的欄位傳遞到視圖，避免資料庫模型過於直接地暴露給用戶界面
                2. 如果需要對 TInventoryMain 或 TInventoryDetail 進行結構調整（例如新增欄位或移除欄位）
                   不需要直接修改視圖中的資料結構，只需在 ViewModel 中進行調整。
            */


            // 查詢 InventoryMain 和 InventoryDetails
            var inventoryMain = _context.TInventoryMains.FirstOrDefault();
            var inventoryDetails = _context.TInventoryDetails.ToList();

            // 將 TInventoryMain 轉換為 CInventoryMainWrap
            var inventoryMainWrap = new CInventoryMainWrap
            {
                FId = inventoryMain.FId,
                FInventoryNo = inventoryMain.FInventoryNo,
                FBaselineDate = inventoryMain.FBaselineDate,
                FCreatedAt = inventoryMain.FCreatedAt,
                FCreatorId = inventoryMain.FCreatorId,
                FEditorId = inventoryMain.FEditorId
            };

            // 將 TInventoryDetail 轉換為 CInventoryDetailWrap
            var inventoryDetailWraps = inventoryDetails.Select(detail => new CInventoryDetailWrap
            {
                FId = detail.FId,
                FInventoryNo = detail.FInventoryNo,
                FProductId = detail.FProductId,
                FProductName = detail.FProductName,
                FSystemQuantity = detail.FSystemQuantity,
                FActualQuantity = detail.FActualQuantity,
                FDifferenceQuantity = detail.FDifferenceQuantity,
                FAmount = detail.FAmount,
                FRemark = detail.FRemark,
                FEditorId = detail.FEditorId
            }).ToList();

            // 創建 ViewModel 並傳遞到視圖
            var viewModel = new CInventoryViewModel
            {
                InventoryMain = inventoryMainWrap,
                InventoryDetails = inventoryDetailWraps
            };

            return View(viewModel);
        }

        //GET Inventory/InventoryAdjustment
        public IActionResult InventoryAdjustment()
        {
            // 查詢 InventoryMain 和 InventoryDetails
            var inventoryMain = _context.TInventoryMains.FirstOrDefault();
            var inventoryDetails = _context.TInventoryDetails.ToList();

            // 將 TInventoryMain 轉換為 CInventoryMainWrap
            var inventoryMainWrap = new CInventoryMainWrap
            {
                FId = inventoryMain.FId,
                FInventoryNo = inventoryMain.FInventoryNo,
                FBaselineDate = inventoryMain.FBaselineDate,
                FCreatedAt = inventoryMain.FCreatedAt,
                FCreatorId = inventoryMain.FCreatorId,
                FEditorId = inventoryMain.FEditorId
            };

            // 將 TInventoryDetail 轉換為 CInventoryDetailWrap
            var inventoryDetailWraps = inventoryDetails.Select(detail => new CInventoryDetailWrap
            {
                FId = detail.FId,
                FInventoryNo = detail.FInventoryNo,
                FProductId = detail.FProductId,
                FProductName = detail.FProductName,
                FSystemQuantity = detail.FSystemQuantity,
                FActualQuantity = detail.FActualQuantity,
                FDifferenceQuantity = detail.FDifferenceQuantity,
                FAmount = detail.FAmount,
                FRemark = detail.FRemark,
                FEditorId = detail.FEditorId
            }).ToList();

            // 創建 ViewModel 並傳遞到視圖
            var viewModel = new CInventoryViewModel
            {
                InventoryMain = inventoryMainWrap,
                InventoryDetails = inventoryDetailWraps
            };

            return View(viewModel);
        }
    }
}
