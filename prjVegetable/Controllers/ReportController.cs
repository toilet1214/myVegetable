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
            var list = _context.TReports
                       .Select(p => new CReportWrap { report = p })
                       .ToList();

            return View(list);
        }
        public IActionResult Details(int id)
        {
            var report = _context.TReports.FirstOrDefault(p => p.FId == id);
            if (report == null)
                return NotFound();

            var wrap = new CReportWrap { report = report };
            return View(wrap);
        }
    }
}
