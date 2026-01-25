using Microsoft.EntityFrameworkCore;
using OfficeMaster.Data;
using OfficeMaster.Interface;
using OfficeMaster.Models;
using OfficeMaster.Models.Enum;

namespace OfficeMaster.Service;

public class ConferenceRoomService : IConferenceRoomService
{
    private readonly ApplicationDbContext _context;

    public ConferenceRoomService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ConferenceRoom>> GetRoomsAsync(string? search, int? minCapacity, int? maxCapacity, RoomType? type, bool? hasProjector)
    {
        var query = _context.ConferenceRooms.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r => r.Name.ToLower().Contains(search.ToLower()));
        }
        
        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (maxCapacity.HasValue)
            query = query.Where(r => r.Capacity <= maxCapacity.Value);
        
        if (hasProjector is true)
        {
            query = query.Where(r => r.HasProjector == true);
        }
        
        return await query.ToListAsync();
    }
}