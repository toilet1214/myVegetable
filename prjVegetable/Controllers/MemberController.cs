using Microsoft.AspNetCore.Mvc;

namespace prjVegetable.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Info()
        {
            return View();
        }

        public IActionResult Forgot()
        {
            return View();
        }




    }
}
