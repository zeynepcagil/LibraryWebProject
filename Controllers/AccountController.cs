using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Library_Project.Models;

namespace Library_Project.Controllers;

public class AccountController : Controller
{
    private readonly Context _context;

    public AccountController(Context context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.Users
            .FirstOrDefault(u => u.EMAIL == model.Email && u.PASSWORD == model.Password);

        if (user == null)
        {
            ModelState.AddModelError("", "Email veya şifre hatalı");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.NAME),
            new Claim(ClaimTypes.Email, user.EMAIL),
            new Claim("UserId", user.USER_ID.ToString()),
            new Claim("IsAdmin", user.RANK ? "1" : "0")
        };

        var identity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // aynı email veya username var mı
        bool exists = _context.Users.Any(u =>
            u.EMAIL == model.EMAIL || u.USERNAME == model.USERNAME);

        if (exists)
        {
            ModelState.AddModelError("", "Bu email veya kullanıcı adı zaten kayıtlı");
            return View(model);
        }

        var user = new User
        {
            USERNAME = model.USERNAME,
            EMAIL = model.EMAIL,
            PASSWORD = model.PASSWORD, // şimdilik plain (sonra hashleriz)
            NAME = model.NAME,
            SURNAME = model.SURNAME,
            BIRTHDATE = model.BIRTHDATE,
            PHONE = model.PHONE,
            RANK = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // otomatik login
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.NAME),
        new Claim(ClaimTypes.Email, user.EMAIL),
        new Claim("UserId", user.USER_ID.ToString()),
        new Claim("IsAdmin", user.RANK ? "1" : "0")

    };

        var identity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }
}
