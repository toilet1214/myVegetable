using Microsoft.EntityFrameworkCore;
using prjVegetable.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DbVegetableContext>(
    options =>options.UseSqlServer(
        builder.Configuration.GetConnectionString("dbVegetable")
        ));
builder.Services.AddControllers().AddNewtonsoftJson(); // ? �T�O�䴩 JObject

builder.Services.AddMemoryCache(); // ? ���U�O����֨�

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "�A��GoogleClientId";
    googleOptions.ClientSecret = "�A��GoogleClientSecret";
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
