using Microsoft.AspNetCore.Mvc;

namespace prjVegetable.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
