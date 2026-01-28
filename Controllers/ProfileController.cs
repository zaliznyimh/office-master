using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeMaster.Interface;
using OfficeMaster.Models;
using OfficeMaster.ViewModels.Profile;

namespace OfficeMaster.Controllers;

public class ProfileController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IProfileService _profileService;

    public ProfileController(UserManager<User> userManager, IProfileService profileService)
    {
        _userManager = userManager;
        _profileService = profileService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");
        
        var reservations = await _profileService.GetUserReservations(user.Id);

        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            CompanyName = user.CompanyName,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email!,
            MyReservations = reservations
        };

        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(ProfileViewModel model)
    {
        if (!long.TryParse(_userManager.GetUserId(User), out var userId) || userId <= 0)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            model.MyReservations = await _profileService.GetUserReservations(userId);
            return View("Index", model);
        }

        var result = await _profileService.UpdateUserProfileAsync(userId, model);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        model.MyReservations = await _profileService.GetUserReservations(userId);
        return View("Index", model);
    }
}