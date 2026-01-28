using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeMaster.Interface;

namespace OfficeMaster.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IReservationService _reservationService;
    
    public DashboardController(IDashboardService dashboardService, IReservationService reservationService)
    {
        _dashboardService = dashboardService;
        _reservationService = reservationService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = await _dashboardService.GetDashboardDataAsync();
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(long id)
    {
        var result = await _reservationService.ApproveReservationAsync(id);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.ErrorMessage;
        }
        else
        {
            TempData["Success"] = "Reservation Approved";
        }
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Reject(long id)
    {
        var success = await _reservationService.DeclineReservationAsync(id);
        
        if (success)
        {
            TempData["Success"] = "Reservation Declined";
        }
        else
        {
            TempData["Error"] = "Reservation not found";
        }
        
        return RedirectToAction(nameof(Index));
    }
}