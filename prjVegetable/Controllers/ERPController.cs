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
        public IActionResult ProductBuying()
        {
            return View();
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