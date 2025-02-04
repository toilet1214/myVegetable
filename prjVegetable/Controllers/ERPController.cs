using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
            int TotalOrdersYear = _VegetableContext.TOrders.Count(o => o.FOrderAt.Year == DateTime.Now.Year);
            int TotalOrdersMonth = _VegetableContext.TOrders.Count(o => o.FOrderAt.Month == DateTime.Now.Month && o.FOrderAt.Year == DateTime.Now.Year);
            int TotalOrdersDay = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date);
            int TotalOrdersLastYear = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date.AddYears(-1));
            int TotalOrdersLastMonth = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date.AddMonths(-1));
            int TotalOrdersLastDay = _VegetableContext.TOrders.Count(o => o.FOrderAt.Date == DateTime.Now.Date.AddDays(-1));
            if (TotalOrdersLastYear != 0)
            {
                ViewBag.OrdersPercentageYear = Math.Round(((decimal)(TotalOrdersYear / TotalOrdersLastYear - 1) * 100), 1);
            }
            else ViewBag.OrdersPercentageYear = 100;
            if (TotalOrdersLastMonth != 0)
            {
                ViewBag.OrdersPercentageMonth = Math.Round((decimal)((TotalOrdersMonth / TotalOrdersLastMonth - 1) * 100), 1);
            }
            else ViewBag.OrdersPercentageMonth = 100;
            if (TotalOrdersLastDay != 0)
            {
                ViewBag.OrdersPercentageDay = Math.Round(((decimal)(TotalOrdersDay / TotalOrdersLastDay - 1) * 100), 1);
            }
            else ViewBag.OrdersPercentageDay = 100;

            ViewBag.VisitorsPercentageDay = 100;
            ViewBag.VisitorsPercentageMonth = 80;
            ViewBag.VisitorsPercentageYear = 70;
            var viewmodel = new CERPIndexViewModel
            {
                TotalMembers = _VegetableContext.TPeople.Count(p => p.FPermission == 0),
                TotalVisitorsYear = 100,//待修改
                TotalVisitorsMonth = 10,//待修改
                TotalVisitorsDay = 1,//待修改
                TotalOrdersYear = TotalOrdersYear,
                TotalOrdersMonth = TotalOrdersMonth,
                TotalOrdersDay = TotalOrdersDay,
                //BestSellingProductYear = ""  ,
                //BestSellingProductMonth ="",
                //BestSellingProductDay ="",

                //MostPopularProductYear ="",
                //MostPopularProductMonth ="",
                //MostPopularProductDay ="",

                //BestSellingClassYear ="",
                //BestSellingClassMonth ="",
                //BestSellingClassDay =""
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