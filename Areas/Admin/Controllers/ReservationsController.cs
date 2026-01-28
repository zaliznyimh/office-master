using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeMaster.Interface;
using OfficeMaster.Models.Enum;

namespace OfficeMaster.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ReservationsController : Controller
{
    private readonly IReservationService _reservationService;
    
    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    
    public async Task<IActionResult> Index(string? search, ReservationStatus? status)
    {
        var bookings = await _reservationService.GetReservationsAsync(search, status);
        
        ViewBag.CurrentSearch = search;
        ViewBag.CurrentStatus = status;

        return View(bookings);
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
            TempData["Success"] = "Reservation approved successfully.";
        }

        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> Decline(long id)
    {
        var success = await _reservationService.DeclineReservationAsync(id);
        
        if (success)
        {
            TempData["Success"] = "Reservation rejected.";
        }
        else
        {
            TempData["Error"] = "Reservation not found.";
        }

        return RedirectToAction(nameof(Index));
    }
}