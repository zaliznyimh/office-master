using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeMaster.Interface;
using OfficeMaster.Models;
using OfficeMaster.Models.Enum;
using OfficeMaster.ViewModels.Rooms;

namespace OfficeMaster.Controllers;

[Authorize]
public class ConferenceRoomController : Controller
{
    private readonly IConferenceRoomService _conferenceRoomService;
    private readonly UserManager<User> _userManager;
    
    public ConferenceRoomController(IConferenceRoomService conferenceRoomService, UserManager<User> userManager)
    {
        _conferenceRoomService = conferenceRoomService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int? minCapacity, int? maxCapacity, RoomType? type,
        bool? hasProjector, DateTime? startDate, DateTime? endDate)
    {
        var rooms = await _conferenceRoomService.GetRoomsAsync(search, minCapacity, maxCapacity, type, hasProjector,
            startDate, endDate);

        FillViewBag(search, minCapacity, maxCapacity, type, hasProjector, startDate, endDate);

        return View(rooms);
    }

    [HttpGet]
    public async Task<IActionResult> FilterRooms(string? search, int? minCapacity, int? maxCapacity, RoomType? type,
        bool? hasProjector, bool? hasWhiteboard, bool? hasVideoConf, DateTime? startDate, DateTime? endDate)
    {
        var rooms = await _conferenceRoomService.GetRoomsAsync(search, minCapacity, maxCapacity, type, hasProjector,
            startDate, endDate);
        
        FillViewBag(search, minCapacity, maxCapacity, type, hasProjector, startDate, endDate);
        
        return PartialView("_RoomList", rooms);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var room = await _conferenceRoomService.GetRoomByIdAsync(id);
        if (room == null) return NotFound();
        
        var now = DateTime.Now;
        var nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);

        var viewModel = new RoomDetailsViewModel
        {
            Room = room,
            RoomId = room.Id,
            StartTime = nextHour,
            EndTime = nextHour.AddHours(2)
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(RoomDetailsViewModel model)
    {
        var room = await _conferenceRoomService.GetRoomByIdAsync(model.RoomId);
        if (room == null) return NotFound();
        
        model.Room = room;

        if (!ModelState.IsValid) return View("Details", model);
        
        if (model.StartTime >= model.EndTime)
        {
            ModelState.AddModelError("", "End time must be after start time.");
            return View("Details", model);
        }

        if (model.StartTime < DateTime.Now)
        {
            ModelState.AddModelError("", "Cannot book in the past.");
            return View("Details", model);
        }
        
        var isAvailable = await _conferenceRoomService.IsRoomAvailableAsync(model.RoomId, model.StartTime, model.EndTime);
        if (!isAvailable)
        {
            ModelState.AddModelError("", "This room is already booked for the selected time.");
            return View("Details", model);
        }
        
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return RedirectToAction("Login", "Account");

        var result = await _conferenceRoomService.BookRoomAsync(room, user.Id, model);
        if (!result)
        {
            ModelState.AddModelError("", "Something went wrong. Try again later");
            return View("Details", model);
        }

        TempData["SuccessMessage"] = "Request sent! Waiting for Admin approval.";
        return RedirectToAction("Index", "Profile");
    }
    
    [HttpGet("GetRoomEvents/{id}")]
    public async Task<IActionResult> GetRoomEvents(long id)
    {
        var reservations = await _conferenceRoomService.GetConferenceRoomCalendar(id);

        return Json(reservations);
    }
    
    private void FillViewBag(string? search, int? minCapacity, int? maxCapacity, RoomType? type, bool? hasProjector,
        DateTime? start, DateTime? end)
    {
        ViewBag.CurrentSearch = search;
        ViewBag.MinCapacity = minCapacity;
        ViewBag.MaxCapacity = maxCapacity;
        ViewBag.CurrentType = type;
        ViewBag.CurrentProjector = hasProjector;
        ViewBag.StartDate = start?.ToString("yyyy-MM-ddTHH:mm");
        ViewBag.EndDate = end?.ToString("yyyy-MM-ddTHH:mm");
    }
}