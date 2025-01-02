using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using System.Diagnostics;

namespace prjVegetable.Controllers
{
    public class ProductController : Controller
    {

        public IActionResult ProductItem()
        {
            DbVegetableContext db = new DbVegetableContext();

            TProduct p = new TProduct();
            CProductWrap x = new CProductWrap() { product=p};
            
            return View(x);
        }
    }
}
