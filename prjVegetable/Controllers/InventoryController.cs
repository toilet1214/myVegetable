using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class InventoryController : Controller
    {
        public readonly DbVegetableContext _VegetableContext;

        public InventoryController(DbVegetableContext VegetableContext)
        {
            _VegetableContext = VegetableContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
