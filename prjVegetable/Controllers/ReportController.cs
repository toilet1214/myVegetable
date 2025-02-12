using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class ReportController : Controller
    {
        private readonly DbVegetableContext _context;

        public ReportController(DbVegetableContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));  // 添加防止 null 的檢查;
        }
        //public IActionResult List()
        //{
        //    string keyword = vm.txtKeyword;
        //    IEnumerable<TPerson> datas = null;
        //    if (string.IsNullOrEmpty(keyword))
        //    {
        //        datas = from p in _context.TPeople
        //                select p;
        //    }
        //    else
        //    {
        //        datas = _context.TPeople.Where(p =>
        //        p.FName.Contains(keyword) ||
        //        p.FPhone.Contains(keyword) ||
        //        p.FAddress.Contains(keyword) ||
        //        p.FEmail.Contains(keyword)
        //        );
        //    }
        //    var data = _context.TPeople.ToList();
        //    List<CPersonWrap> list = new List<CPersonWrap>();
        //    foreach (var p in data)
        //    {
        //        list.Add(new CPersonWrap() { person = p });
        //    }
        //    return View(list);

        //}
    }
}
