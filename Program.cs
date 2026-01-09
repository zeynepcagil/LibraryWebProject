using Microsoft.EntityFrameworkCore;
using Library_Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// DB BAĞLANTISI
// NOT: appsettings.json dosyanızda ismin "Context" veya "DefaultConnection" olduğuna emin olun.
// Önceki adımlarda "Context" yapmıştık, hata alırsanız burayı kontrol edin.
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Context") ?? builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC & SESSION SERVİSLERİ
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Servis eklendi ✅

// AUTH (KİMLİK DOĞRULAMA) AYARLARI
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });

// YETKİLENDİRME POLİTİKASI
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("IsAdmin", "1"));
});

var app = builder.Build();

// PIPELINE (İSTEK HATTI)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ---------------------------------------------------------
// !!! EKSİK OLAN KRİTİK SATIR BURASIYDI !!!
// Bu satır olmazsa "Session has not been configured" hatası alırsın.
app.UseSession();
// ---------------------------------------------------------

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();