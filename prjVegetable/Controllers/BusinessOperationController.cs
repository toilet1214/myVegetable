using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using prjVegetable.Models;

namespace prjVegetable.Controllers
{
    public class BusinessOperationController : Controller
    {
        private readonly ILogger<BusinessOperationController> _logger;
        private readonly IWebHostEnvironment _environment;

        public BusinessOperationController(ILogger<BusinessOperationController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult List()
        {
            return View();
        }

        // Session 身份驗證方法
        //private bool CheckUserSession()
        //{
        //    if (int.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER_ID), out int userId) && userId > 0)
        //    {
        //        return true;
        //    }

        //    // 用户未登录，重定向到登录页
        //    return false;
        //}


        // 銷售概況
        public IActionResult Business()
        {
            //if (!CheckUserSession())
            //{
            //    return RedirectToAction("Index", "ERP");
            //}
            return View();
        }

        // 交易紀錄
        public IActionResult Transaction()
        {
            //if (!CheckUserSession())
            //{
            //    return RedirectToAction("Index", "ERP");
            //}
            return View();
        }

        // 銷售商品分類
        public IActionResult Goods()
        {
            //if (!CheckUserSession())
            //{
            //    return RedirectToAction("Index", "ERP");
            //}
            return View();
        }
    }
}
