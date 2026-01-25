using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeMaster.Interface;
using OfficeMaster.Models.Enum;

namespace OfficeMaster.Controllers;

[Authorize]
public class ConferenceRoomController : Controller
{
    private readonly IConferenceRoomService _conferenceRoomService;
    
    public ConferenceRoomController(IConferenceRoomService conferenceRoomService)
    {
        _conferenceRoomService = conferenceRoomService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int? minCapacity, int? maxCapacity, RoomType? type,
        bool? hasProjector)
    {
        var rooms = await _conferenceRoomService.GetRoomsAsync(search, minCapacity, maxCapacity, type, hasProjector);

        ViewBag.CurrentSearch = search;
        
        ViewBag.MinCapacity = minCapacity;
        ViewBag.MaxCapacity = maxCapacity;

        ViewBag.CurrentType = type;
        ViewBag.CurrentProjector = hasProjector;

        return View(rooms);
    }
}