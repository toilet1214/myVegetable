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
            var viewmodel = new CERPIndexViewModel
            {
                TotalMembers =  1           ,
                TotalVisitors =   1         ,
                TotalOrdersYear =   1       ,
                TotalOrdersMonth =1,
                TotalOrdersWeek =1,
                BestSellingProductYear = ""  ,
                BestSellingProductMonth ="",
                BestSellingProductWeek ="",

                MostPopularProductYear ="",
                MostPopularProductMonth ="",
                MostPopularProductWeek ="",

                BestSellingClassYear ="",
                BestSellingClassMonth ="",
                BestSellingClassWeek =""
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