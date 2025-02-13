using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using prjVegetable.ViewModels;
using System.Diagnostics;
using System.Text.Json;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_IS_VERIFIED, user.FisEmailVerified.ToString());
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
            bool exists = _context.TPeople.Any(x => x.FAccount == P.FAccount); // 可以根據需要使用 Email 進行檢查

            if (exists)
            {
                // 傳遞錯誤訊息給視圖
                TempData["ErrorRegister"] = "帳號已經註冊過了！";
                return RedirectToAction("Register"); // 重新導向回註冊頁面
            }

            _context.TPeople.Add(P);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forgot()
        {
            return View();
        }


        [HttpPost]
        [Route("send-verification-email")]
        public async Task<IActionResult> SendVerificationEmail(int personId, string email)
        {
            // 1. 產生 Token
            string token = Guid.NewGuid().ToString();
            DateTime expiration = DateTime.UtcNow.AddMinutes(10); // 設定 10 分鐘過期

            // 2. 存入資料庫
            var verification = new TVerification
            {
                FPersonId = personId,
                Ftoken = token,
                FtokenType = "Verification", // 這是驗證用的 Token
                FtokenSentTime = DateTime.UtcNow,
                FexpirationTime = expiration,
                FisUsed = false
            };

            _context.TVerifications.Add(verification);
            await _context.SaveChangesAsync();

            // 3. 建立驗證連結

            string verifyUrl = $"http://localhost:7251/verify?token={token}";

            // 4. 發送 Email
            var smtpSettings = _config.GetSection("Smtp");
            using (var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
            {
                client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["Username"]),
                    Subject = "Email Verification",
                    Body = $"請點擊以下連結驗證你的 Email: <a href='{verifyUrl}'>驗證 Email</a>",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);
                await client.SendMailAsync(mailMessage);
            }
            return Ok("驗證郵件已發送！");
        }
        // 驗證 Email Token 的 API
        [HttpGet]
        [Route("verify")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            // 1. 查詢 Token 是否存在
            var verification = await _context.TVerifications
                .FirstOrDefaultAsync(v => v.Ftoken == token && !v.FisUsed);

            if (verification == null)
            {
                return Content("驗證失敗：無效的驗證碼或已使用！");
            }

            // 2. 檢查 Token 是否過期
            if (verification.FexpirationTime < DateTime.UtcNow)
            {
                return Content("驗證失敗：驗證碼已過期！");
            }

            // 3. 標記 Token 為已使用
            verification.FisUsed = true;
            verification.FusedTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Content("Email 驗證成功！");
        }

        // 刪除過期的 Token
        public async Task RemoveExpiredTokens()
        {
            var expiredTokens = _context.TVerifications
                .Where(ev => ev.FexpirationTime < DateTime.UtcNow);

            _context.TVerifications.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }

        // 定時清理過期 Token
        public void StartExpiredTokenCleanup()
        {
            var timer = new System.Timers.Timer(3600000); // 每小時執行一次
            timer.Elapsed += async (sender, e) => await RemoveExpiredTokens();
            timer.Start();
        }
    }


}
