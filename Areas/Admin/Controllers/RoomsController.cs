using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeMaster.Interface;
using OfficeMaster.Models.Enum;
using OfficeMaster.ViewModels.Rooms;

namespace OfficeMaster.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class RoomsController : Controller
{
    private readonly IConferenceRoomService _conferenceRoomService;

    public RoomsController(IConferenceRoomService conferenceRoomService)
    {
        _conferenceRoomService = conferenceRoomService;
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
    public IActionResult Create()
    {
        return View(new RoomFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoomFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _conferenceRoomService.CreateRoomAsync(model);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var room = await _conferenceRoomService.GetRoomByIdAsync(id);
        if (room == null) return NotFound();

        var model = new RoomFormViewModel
        {
            Id = room.Id,
            Name = room.Name,
            Description = room.Description,
            Capacity = room.Capacity,
            PricePerHour = room.PricePerHour,
            RoomType = room.RoomType,
            ImageUrl = room.ImageUrl,
            HasProjector = room.HasProjector
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, RoomFormViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var success = await _conferenceRoomService.UpdateRoomAsync(id, model);

            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        var success = await _conferenceRoomService.DeleteRoomAsync(id);

        if (!success) return NotFound();

        return RedirectToAction(nameof(Index));
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