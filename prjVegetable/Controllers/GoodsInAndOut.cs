using Microsoft.AspNetCore.Mvc;

namespace prjVegetable.Controllers
{
    public class GoodsInAndOut : Controller
    {
        public IActionResult GoodsInAndOutIndex()
        {
            return View();
        }
    }
}
