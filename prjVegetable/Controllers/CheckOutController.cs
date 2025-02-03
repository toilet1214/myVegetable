using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly DbVegetableContext _dbContext;
        public IActionResult CheckOutIndex()
        {
            return View();
        }
        
    }
}
