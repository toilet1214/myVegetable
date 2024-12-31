using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(CCustomerWrap p)
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }


        public IActionResult Order()
        {
            return View();
        }

        public IActionResult OrderDetail()
        {
            return View();
        }

    }
}
