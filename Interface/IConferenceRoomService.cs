using OfficeMaster.Models;
using OfficeMaster.Models.Enum;

namespace OfficeMaster.Interface;

public interface IConferenceRoomService
{
    Task<List<ConferenceRoom>> GetRoomsAsync(string? search, int? minCapacity, int? maxCapacity, RoomType? type, bool? hasProjector);
}