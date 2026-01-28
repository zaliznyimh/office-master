using OfficeMaster.Models;
using OfficeMaster.Models.Enum;
using OfficeMaster.ViewModels.Rooms;
using OfficeMaster.ViewModels.Shared;

namespace OfficeMaster.Interface;

public interface IConferenceRoomService
{
    Task<List<ConferenceRoom>> GetRoomsAsync(string? search, int? minCapacity, 
        int? maxCapacity, RoomType? type, bool? hasProjector, DateTime? startDate,
        DateTime? endDate);
    
    Task<ConferenceRoom?> GetRoomByIdAsync(long id);
    Task<bool> IsRoomAvailableAsync(long roomId, DateTime start, DateTime end);
    Task<bool> BookRoomAsync(ConferenceRoom room, long userId, RoomDetailsViewModel model);
    Task<List<CalendarResponse>> GetConferenceRoomCalendar(long roomId);
    
    Task CreateRoomAsync(RoomFormViewModel model);
    Task<bool> UpdateRoomAsync(long id, RoomFormViewModel model);
    Task<bool> DeleteRoomAsync(long id);
}