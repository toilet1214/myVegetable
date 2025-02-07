using Microsoft.AspNetCore.Mvc;

namespace prjVegetable.Controllers
{
    public class IndexCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
