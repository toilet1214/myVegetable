using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class ERPController : Controller
    {

        public readonly DbVegetableContext _VegetableContext;

        public ERPController (DbVegetableContext VegetableContext) 
        {
            _VegetableContext = VegetableContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        //盤點作業 by允7
        public IActionResult inventory()
        {
            ViewBag.AddUrl = Url.Action("AddItem", "Inventory");
            var TInventoryDetail = _VegetableContext.TInventoryDetails.ToList();
            return View(TInventoryDetail);
        }

        public IActionResult inventory_adjustment()
        {
            ViewBag.AddUrl = Url.Action("AddItem", "Inventory");
            var TInventoryDetail = _VegetableContext.TInventoryDetails.ToList();
            return View(TInventoryDetail);
        }

        public IActionResult CardSetting()
        {
            return View();
        }

        public IActionResult CarouselSetting()
        {
            return View();
        }
    }
}