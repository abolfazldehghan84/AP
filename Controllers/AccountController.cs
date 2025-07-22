using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Security.Claims;

namespace project.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // نمایش فرم ورود
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // جستجوی کاربر در نقش‌های مختلف
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == model.Email && s.PasswordHash == model.Password);
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Email == model.Email && t.PasswordHash == model.Password);
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == model.Email && a.PasswordHash == model.Password);

            object user = student ?? (object)teacher ?? admin;

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "ایمیل یا رمز عبور نادرست است.");
                return View(model);
            }

            // تعیین نقش
            string role = user is Admin ? "Admin"
                        : user is Teacher ? "Teacher"
                        : "Student";

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, model.Email),
        new Claim(ClaimTypes.Role, role)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // انتقال به کنترلر مناسب
            return RedirectToAction("Index", role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View(); // فقط ویو AccessDenied.cshtml نمایش داده می‌شود
        }


    }
}
