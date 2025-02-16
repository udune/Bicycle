using Bicycle.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bicycle.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _UserManager;
    private readonly SignInManager<IdentityUser> _SignInManager;
    
    public AccountController(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> SignInManager)
    {
        _UserManager = UserManager;
        _SignInManager = SignInManager;
    }
    
    public IActionResult Register()
    {
        var viewModel = new RegisterViewModel();
        return View(viewModel);
    }

    public IActionResult Login()
    {
        var viewModel = new LoginViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser() { UserName = model.UserName, Email = model.Email };
            user.EmailConfirmed = true;
            var result = await _UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Login");
            
            ModelState.AddModelError("Register", result.Errors.FirstOrDefault().Description);
        }
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _SignInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            
            ModelState.AddModelError("Login", "로그인 실패.");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _SignInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}