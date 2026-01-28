using Microsoft.EntityFrameworkCore;
using OfficeMaster.Data;
using OfficeMaster.Interface;
using OfficeMaster.Models;
using OfficeMaster.Models.Enum;
using OfficeMaster.ViewModels.Rooms;
using OfficeMaster.ViewModels.Shared;

namespace OfficeMaster.Service;

public class ConferenceRoomService : IConferenceRoomService
{
    private readonly ApplicationDbContext _context;

    public ConferenceRoomService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ConferenceRoom>> GetRoomsAsync(string? search, int? minCapacity, int? maxCapacity, RoomType? type, bool? hasProjector, DateTime? startDate,
        DateTime? endDate)
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

        if (type.HasValue)
        {
            query = query.Where(r => r.RoomType == type.Value);
        }
        
        if (startDate.HasValue && endDate.HasValue)
        {
            var utcStartDate = startDate.Value.ToUniversalTime();
            var utcEndDate = endDate.Value.ToUniversalTime();
            
            query = query.Where(room => !room.Reservations.Any(r => 
                utcStartDate < r.EndTime && utcEndDate > r.StartTime &&
                r.Status != ReservationStatus.Cancelled
            ));
        }
            
        return await query.ToListAsync();
    }

    public async Task<ConferenceRoom?> GetRoomByIdAsync(long id)
    {
        return await _context.ConferenceRooms
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> IsRoomAvailableAsync(long roomId, DateTime start, DateTime end)
    {
        var startDate = start.ToUniversalTime();
        var endDate = end.ToUniversalTime();
        
        var overlappingReservations = await _context.Reservations
            .Where(r => r.ConferenceRoomId == roomId)
            .Where(r => r.StartTime < endDate && r.EndTime > startDate)
            .ToListAsync();
        
        bool isTaken = overlappingReservations.Any(r => r.Status == ReservationStatus.Pending);

        return !isTaken;
    }

    public async Task<bool> BookRoomAsync(ConferenceRoom room, long userId, RoomDetailsViewModel model)
    {
        var duration = (model.EndTime - model.StartTime).TotalHours;
        var totalPrice = (decimal)duration * room.PricePerHour;

        var reservation = new Reservation
        {
            ConferenceRoomId = room.Id,
            UserId = userId,
            StartTime = model.StartTime.ToUniversalTime(),
            EndTime = model.EndTime.ToUniversalTime(),
            TotalPrice = totalPrice,
            Status = ReservationStatus.Pending
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<CalendarResponse>> GetConferenceRoomCalendar(long roomId)
    {
        var events = await _context.Reservations
            .Where(r => r.ConferenceRoomId == roomId)
            .Where(r => r.Status == ReservationStatus.Approved)
            .Select(r => new CalendarResponse
            {
                Title = "Booked",
                Start = r.StartTime,
                End = r.EndTime
            })
            .ToListAsync();

        return events;
    }

    public async Task CreateRoomAsync(RoomFormViewModel model)
    {
        var room = new ConferenceRoom
        {
            Name = model.Name,
            Description = model.Description,
            Capacity = model.Capacity,
            PricePerHour = model.PricePerHour,
            RoomType = model.RoomType,
            ImageUrl = model.ImageUrl,
            HasProjector = model.HasProjector
        };

        _context.ConferenceRooms.Add(room);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateRoomAsync(long id, RoomFormViewModel model)
    {
        var room = await _context.ConferenceRooms.FindAsync(id);
        if (room == null) return false;
        
        room.Name = model.Name;
        room.Description = model.Description;
        room.Capacity = model.Capacity;
        room.PricePerHour = model.PricePerHour;
        room.RoomType = model.RoomType;
        room.ImageUrl = model.ImageUrl;
        room.HasProjector = model.HasProjector;

        _context.Update(room);
        await _context.SaveChangesAsync();
    
        return true;
    }

    public async Task<bool> DeleteRoomAsync(long id)
    {
        var room = await _context.ConferenceRooms.FindAsync(id);
        if (room == null) return false;

        _context.ConferenceRooms.Remove(room);
        await _context.SaveChangesAsync();
    
        return true;
    }
}