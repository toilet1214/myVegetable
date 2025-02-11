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
        public IActionResult List()
        {
            var datas = from t in _context.TReports select t;
            List<CReportWrap> list = new List<CReportWrap>();
            foreach (var t in datas)
                list.Add(new CReportWrap() { report = t });
            return View(list);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(List));
            }

            var treport = await _context.TReports
                .FirstOrDefaultAsync(m => m.FId == id);
            if (treport == null)
            {
                return RedirectToAction(nameof(List));
            }

            return View(new CReportWrap() { report = treport });
        }
    }
}
