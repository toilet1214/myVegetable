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
builder.Services.AddControllers().AddNewtonsoftJson(); // ? �T�O�䴩 JObject

builder.Services.AddMemoryCache(); // ? ���U�O����֨�

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
    options.Cookie.Name = "VegeMan.AuthCookie";  // �]�w�A�� Cookie �W��
    options.Cookie.HttpOnly = true;                  // �T�O Cookie �u��Ѧ��A��Ū���A�קK JavaScript �X��
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;  // �ھڽШD��ĳ (http �� https) �M�w Cookie �O�_�ȳz�L HTTPS �ǿ�
    options.Cookie.SameSite = SameSiteMode.Lax;      // �]�w SameSite �ݩʡALax �O�@�ӱ`�����ﶵ�A����󯸽ШD���y (CSRF) ����
    options.Cookie.MaxAge = TimeSpan.FromHours(8); ;    // �]�w Cookie �����Ĵ�
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "787695587602-c0sg85tlcrteh4r27hhdiqsgtavdstdl.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
    googleOptions.CallbackPath = "/signin-google"; // �w�]�^�ո��|
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
