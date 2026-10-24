using System.Security.Claims;
using AspNetCoreMvc.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvc.Controllers;

public class AccountController : Controller
{
    private readonly UserRepository _users;
    public AccountController(UserRepository users) => _users = users;

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Tasks");
        return View();
    }

    // "Google ilə daxil ol" düyməsi bura gəlir və Google-a yönləndirir
    [HttpPost]
    public IActionResult GoogleLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleResponse))
        };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    // Google-dan geri qayıdanda
    [HttpGet]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded) return RedirectToAction(nameof(Login));

        var googleId = result.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var email = result.Principal.FindFirstValue(ClaimTypes.Email) ?? "";
        var name = result.Principal.FindFirstValue(ClaimTypes.Name);

        // DB-də user-i tap və ya yarat
        var user = await _users.GetOrCreateAsync(googleId, email, name);

        // Öz DB Id-miz ilə yeni cookie yaradırıq
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Tasks");
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}
