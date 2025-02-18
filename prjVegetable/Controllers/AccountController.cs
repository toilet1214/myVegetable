using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Text.Json;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace prjVegetable.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbVegetableContext _context;
        private readonly IConfiguration _config;
        public AccountController(IConfiguration config, DbVegetableContext context)
        {
            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost]
        public IActionResult Login(CLoginViewModel vm, string returnUrl)
        {
            TPerson user = (new DbVegetableContext()).TPeople.FirstOrDefault(t => t.FAccount.Equals(vm.txtAccount) && t.FPassword.Equals(vm.txtPassword));
            if (user != null && user.FPassword.Equals(vm.txtPassword))
            {
                string json = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_PERMISSION, user.FPermission.ToString());
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_ID, user.FId.ToString());
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_IS_VERIFIED, user.FIsVerified.ToString());
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
        public async Task<IActionResult> Register(TPerson P)
        {
            bool exists = _context.TPeople.Any(x => x.FAccount == P.FAccount);

            if (exists)
            {
                TempData["ErrorRegister"] = "這個信箱已經註冊過了！";
                return RedirectToAction("Register");
            }

            // 產生驗證 Token
            P.FVerificationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            P.FIsVerified = false;

            _context.TPeople.Add(P);
            _context.SaveChanges();

            // 發送驗證信
            await SendVerificationEmail(P.FAccount, P.FVerificationToken);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult VerifyEmail(string email, string token)
        {
            var user = _context.TPeople.FirstOrDefault(u => u.FAccount == email);
            if (user == null || user.FVerificationToken != token)
            {
                return Content("驗證失敗，請確認連結是否正確");
            }

            user.FIsVerified = true;
            user.FVerificationToken = null;
            _context.SaveChanges();

            return Content("驗證成功，您現在可以登入");
        }

        private async Task SendVerificationEmail(string email, string token)
        {
            var verificationUrl = Url.Action("VerifyEmail", "Account",
                new { email = email, token = token }, Request.Scheme);

            var body = $"請點擊 <a href='{verificationUrl}'>這裡</a> 來驗證您的帳戶。";

            using var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("vege.man05@gmail.com", "vrdg elte degt sowm"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("vege.man05@gmail.com"),
                Subject = "請驗證您的電子郵件",
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
        public async Task<IActionResult> ResendVerificationEmail(string email)
        {
            var user = _context.TPeople.FirstOrDefault(u => u.FAccount == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "找不到此 Email";
                return RedirectToAction("Forgot");
            }
            if (user.FIsVerified)
            {
                TempData["ErrorMessage"] = "此帳戶已驗證";
                return RedirectToAction("Index", "Home");
            }

            // 重新產生驗證 Token
            user.FVerificationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            _context.SaveChanges();

            await SendVerificationEmail(user.FAccount, user.FVerificationToken);

            TempData["SuccessMessage"] = "驗證信已重新發送，請檢查您的信箱";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Forgot()
        {
            return View();
        }


    }


}
