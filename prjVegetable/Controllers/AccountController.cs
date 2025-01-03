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
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_PERMISSION, user.FPermission);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_ID, user.FId.ToString());
                TempData["LogedIn321"] = HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER);
                return Redirect(returnUrl);
            }
            TempData["ErrorMessage"] = "帳號或密碼錯誤，請再試一次";
            return Redirect(returnUrl);
        }
    }
}
