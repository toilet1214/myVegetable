using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddDbContext<DbVegetableContext>(
    options =>options.UseSqlServer(
        builder.Configuration.GetConnectionString("dbVegetable")
        ));
builder.Services.AddControllers().AddNewtonsoftJson(); // ? 確保支援 JObject

builder.Services.AddMemoryCache(); // ? 註冊記憶體快取

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "VegeMan.AuthCookie";  // 設定你的 Cookie 名稱
    options.Cookie.HttpOnly = true;                  // 確保 Cookie 只能由伺服器讀取，避免 JavaScript 訪問
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;  // 根據請求協議 (http 或 https) 決定 Cookie 是否僅透過 HTTPS 傳輸
    options.Cookie.SameSite = SameSiteMode.Lax;      // 設定 SameSite 屬性，Lax 是一個常見的選項，防止跨站請求偽造 (CSRF) 攻擊
    options.Cookie.MaxAge = TimeSpan.FromHours(8); ;    // 設定 Cookie 的有效期
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "787695587602-c0sg85tlcrteh4r27hhdiqsgtavdstdl.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
    googleOptions.CallbackPath = "/signin-google"; // 預設回調路徑
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
