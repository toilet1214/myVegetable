using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using prjVegetable.ViewModels;

namespace prjVegetable.Controllers
{
    public class ERPController : Controller
    {

        public readonly DbVegetableContext _VegetableContext;

        public ERPController(DbVegetableContext VegetableContext)
        {
            _VegetableContext = VegetableContext;
        }

        public IActionResult Index()
        {
            int TotalOrdersYear = _VegetableContext.TOrders.Count(o => o.FOrderAt >= DateTime.Now.AddYears(-1));
            int TotalOrdersMonth = _VegetableContext.TOrders.Count(o => o.FOrderAt >= DateTime.Now.AddMonths(-1));
            int TotalOrdersWeek = _VegetableContext.TOrders.Count(o => o.FOrderAt >= DateTime.Now.AddDays(-7));

            var viewmodel = new CERPIndexViewModel
            {
                TotalMembers = _VegetableContext.TPeople.Count(p => p.FPermission == 0),
                TotalVisitors = 1,//待修改
                TotalOrdersYear = TotalOrdersYear,
                TotalOrdersMonth = TotalOrdersMonth,
                TotalOrdersWeek = TotalOrdersWeek,
                //BestSellingProductYear = ""  ,
                //BestSellingProductMonth ="",
                //BestSellingProductWeek ="",

                //MostPopularProductYear ="",
                //MostPopularProductMonth ="",
                //MostPopularProductWeek ="",

                //BestSellingClassYear ="",
                //BestSellingClassMonth ="",
                //BestSellingClassWeek =""
            };
            return View(viewmodel);
        }
        public IActionResult ProductBuying()
        {
            return View();
        }

        //盤點作業 by允7
        public IActionResult Inventory()
        {
            ViewBag.AddUrl = Url.Action("AddItem", "Inventory");
            var TInventoryDetail = _VegetableContext.TInventoryDetails.ToList();
            return View(TInventoryDetail);
        }

        public IActionResult InventoryAdjustment()
        {
            ViewBag.AddUrl = Url.Action("AddItem", "Inventory");
            var TInventoryDetail = _VegetableContext.TInventoryDetails.ToList();
            return View(TInventoryDetail);
        }

        public IActionResult CardSetting()
        {
            return View();
        }
        public IActionResult ProductSelling()
        {
            return View();
        }

        public IActionResult CarouselSetting()
        {
            return View();
        }
    }
}