using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeMaster.Models;
using OfficeMaster.ViewModels.Auth;

namespace OfficeMaster.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
        {
            return View(model);
        }
        
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser is not null)
        {
            ModelState.AddModelError(string.Empty, "Пользователь с таким Email уже существует.");
            return View(model);
        }
        
        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            CompanyName = model.CompanyName ?? string.Empty,
            DateOfBirth = DateTime.SpecifyKind(model.DateOfBirth, DateTimeKind.Utc) 
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "ConferenceRoom");
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> CheckEmailAvailability(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Json(new { isTaken = false });
        }

        var user = await _userManager.FindByEmailAsync(email);
        
        return Json(new {isTaken = user!=null});
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (returnUrl is null)
        {
            returnUrl = "/ConferenceRoom";
        }

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                
                return RedirectToAction("Index", "ConferenceRoom");
            }
        
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        
        ViewData["ReturnUrl"] = returnUrl;
    
        return View(model);
    }
    
    [HttpPost] 
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}