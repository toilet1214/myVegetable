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

        // 登入
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm, string returnUrl)
        {
            TPerson user = _context.TPeople.FirstOrDefault(t =>
                t.FAccount == vm.txtAccount && t.FPassword == vm.txtPassword);

            if (user != null)
            {
                string json = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_PERMISSION, user.FPermission.ToString());
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_ID, user.FId.ToString());
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER_IS_VERIFIED, user.FIsVerified.ToString());

                TempData["IsLogIn"] = HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER);
                TempData["WelcomeMessage"] = $"歡迎回來，{user.FAccount}！";
                return Redirect(returnUrl);
            }

            TempData["LoginFail"] = "登入失敗，請重試一次";
            return Redirect(returnUrl);
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear(); // 清除所有 Session

            TempData["LogoutMessage"] = "您已成功登出";
            return RedirectToAction("Index", "Home");
        }
        // 註冊
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(TPerson P)
        {
            if (_context.TPeople.Any(x => x.FAccount == P.FAccount))
            {
                TempData["ErrorRegister"] = "這個信箱已經註冊過了！";
                return RedirectToAction("Register");
            }

            P.FIsVerified = false;
            _context.TPeople.Add(P);
            await _context.SaveChangesAsync(); // 先存入資料庫獲取 FId

            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            var verification = new TVerification
            {
                FPersonId = P.FId,
                FToken = token,
                FTokenType = "驗證",
                FExpirationTime = DateTime.UtcNow.AddMinutes(10),
                FIsUsed = false
            };

            _context.TVerifications.Add(verification);
            await _context.SaveChangesAsync();

            // 發送驗證信，檢查 Email 是否有效
            var emailSent = await SendVerificationEmail(P.FAccount, token);
            if (!emailSent)
            {
                TempData["ErrorRegister"] = "電子郵件地址無效或發送失敗，請確認後重新註冊。";
                return RedirectToAction("Register");
            }

            TempData["SuccessRegister"] = "註冊成功！請檢查您的信箱並完成電子郵件驗證。";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ResendVerificationEmail()
        {
            // 確保輸入的 Email 不為空

            // 查找用戶
            var user = JsonSerializer.Deserialize<TPerson>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            var userId = user.FId;               // 取得使用者 ID



            // 產生新的驗證 Token
            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            // 刪除舊的未使用 Token（確保只有一個有效的驗證碼）
            var oldTokens = _context.TVerifications
                .Where(v => v.FPersonId == userId && v.FTokenType == "驗證" && !v.FIsUsed);
            _context.TVerifications.RemoveRange(oldTokens);

            // 建立新的 Token
            var verification = new TVerification
            {
                FPersonId = userId,
                FToken = token,
                FTokenType = "驗證",
                FExpirationTime = DateTime.UtcNow.AddMinutes(10),
                FIsUsed = false
            };

            _context.TVerifications.Add(verification);
            await _context.SaveChangesAsync();

            // 發送驗證信
            var emailSent = await SendVerificationEmail(user.FAccount, token);
            if (!emailSent)
            {
                TempData["ErrorResend"] = "無法發送驗證信，請稍後再試。";
                return BadRequest(emailSent);
            }

            TempData["SuccessResend"] = "驗證信已重新發送，請檢查您的信箱。";
            return Ok(emailSent);
        }

        // 驗證 Email
        public IActionResult VerifyEmail(string email, string token)
        {
            var verification = _context.TVerifications.FirstOrDefault(v =>
                v.FToken == token && v.FTokenType == "驗證");

            if (verification == null || verification.FIsUsed || verification.FExpirationTime < DateTime.UtcNow)
            {
                return Content("驗證失敗，請確認連結是否正確");
            }

            var user = _context.TPeople.FirstOrDefault(u => u.FId == verification.FPersonId && u.FAccount == email);

            if (user == null)
            {
                return Content("驗證失敗，請確認連結是否正確");
            }

            verification.FIsUsed = true;
            verification.FUsedTime = DateTime.UtcNow;
            user.FIsVerified = true;
            _context.SaveChanges();

            return Content("驗證成功，您現在可以登入");
        }

        // 發送驗證 Email
        private async Task<bool> SendVerificationEmail(string email, string token)
        {
            try
            {
                // 確認 Email 格式是否正確
                if (!IsValidEmail(email))
                {
                    return false;
                }

                var verificationUrl = Url.Action("VerifyEmail", "Account",
                    new { email = email, token = token }, Request.Scheme);

                var body = $"請點擊 <a href='{verificationUrl}'>這裡</a> 來驗證您的帳戶。";

                await SendEmail(email, "請驗證您的電子郵件", body);
                return true; // 發送成功
            }
            catch (SmtpFailedRecipientException)
            {
                return false;
            }
            catch (SmtpException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        // 忘記密碼
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = _context.TPeople.FirstOrDefault(u => u.FAccount == email);
            if (user == null)
            {
                TempData["ErrorForgot"] = "此信箱未註冊！";
                return RedirectToAction("ForgotPassword");
            }

            // 產生驗證 Token
            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            // 新增驗證 Token 記錄
            var verification = new TVerification
            {
                FPersonId = user.FId,
                FToken = token,
                FTokenType = "忘記密碼",
                FExpirationTime = DateTime.UtcNow.AddMinutes(10),
                FIsUsed = false
            };

            _context.TVerifications.Add(verification);
            await _context.SaveChangesAsync();

            // 發送重設密碼郵件
            await SendResetPasswordEmail(email, token);

            TempData["SuccessForgot"] = "請檢查您的電子郵件以重設密碼。";
            return RedirectToAction("ForgotPassword");
        }


        private async Task SendResetPasswordEmail(string email, string token)
        {
            var resetUrl = Url.Action("ResetPassword", "Account",
                new { email = email, token = token }, Request.Scheme);

            var body = $"請點擊 <a href='{resetUrl}'>這裡</a> 來重設您的密碼。";

            await SendEmail(email, "重設您的密碼", body);
        }

        public IActionResult ResetPassword(string email, string token)
        {
            var verification = _context.TVerifications.FirstOrDefault(v =>
                v.FToken == token && v.FTokenType == "忘記密碼");

            if (verification == null || verification.FIsUsed || verification.FExpirationTime < DateTime.UtcNow)
            {
                return Content("密碼重設連結無效或已過期");
            }

            var vm = new ResetPasswordViewModel { Email = email, Token = token };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            var verification = _context.TVerifications.FirstOrDefault(v =>
                v.FToken == vm.Token && v.FTokenType == "忘記密碼");

            if (verification == null || verification.FIsUsed || verification.FExpirationTime < DateTime.UtcNow)
            {
                return Content("密碼重設連結無效或已過期");
            }

            var user = _context.TPeople.FirstOrDefault(u => u.FId == verification.FPersonId && u.FAccount == vm.Email);
            if (user == null)
            {
                return Content("使用者不存在");
            }

            user.FPassword = vm.NewPassword;
            verification.FIsUsed = true;
            verification.FUsedTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Content("密碼已成功重設，請使用新密碼登入");
        }

        // 發送 Email 的通用方法
        private async Task SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_config["Smtp:Host"])
            {
                Port = 587,
                Credentials = new NetworkCredential("vege.man05@gmail.com", "vrdg elte degt sowm"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("vege.man05@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        // Email 格式檢測
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
