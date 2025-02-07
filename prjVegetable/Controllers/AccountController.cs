using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Diagnostics;
using System.Text.Json;


namespace prjVegetable.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm,string returnUrl)
        {
            TPerson user = (new DbVegetableContext()).TPeople.FirstOrDefault(t => t.FAccount.Equals(vm.txtAccount) && t.FPassword.Equals(vm.txtPassword));
            if (user != null && user.FPassword.Equals(vm.txtPassword))
            {
                string json = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);

                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_ID, user.FId.ToString());
                TempData["IsLogIn"] = HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER);
                return Redirect(returnUrl);
            }
            TempData["ErrorMessage"] = "帳號或密碼錯誤，請再試一次";
            return Redirect(returnUrl);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(TPerson P)
        {
            DbVegetableContext db = new DbVegetableContext();
            bool exists = db.TPeople.Any(x => x.FAccount == P.FAccount); // 可以根據需要使用 Email 進行檢查

            if (exists)
            {
                // 傳遞錯誤訊息給視圖
                TempData["ErrorRegister"] = "帳號已經註冊過了！";
                return RedirectToAction("Register"); // 重新導向回註冊頁面
            }

            db.TPeople.Add(P);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forgot()
        {
            return View();
        }
    }
}
