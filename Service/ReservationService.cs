using Microsoft.EntityFrameworkCore;
using OfficeMaster.Data;
using OfficeMaster.Interface;
using OfficeMaster.Models;
using OfficeMaster.Models.Enum;
using OfficeMaster.ViewModels.Shared;

namespace OfficeMaster.Service;

public class ReservationService : IReservationService
{
    private readonly ApplicationDbContext _context;

    public ReservationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetReservationsAsync(string? search, ReservationStatus? status)
    {
        var query = _context.Reservations
            .Include(r => r.User)
            .Include(r => r.ConferenceRoom)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(r =>
                r.User.FirstName.ToLower().Contains(search) ||
                r.User.LastName.ToLower().Contains(search) ||
                r.User.Email!.ToLower().Contains(search));
        }

        if (status.HasValue) query = query.Where(r => r.Status == status.Value);

        return await query.OrderByDescending(r => r.StartTime).ToListAsync();
    }

    public async Task<ApiResult> ApproveReservationAsync(long id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
            return ApiResult.Failure("Reservation not found");
        
        var isTaken = await _context.Reservations.AnyAsync(r =>
            r.ConferenceRoomId == reservation.ConferenceRoomId &&
            r.Status == ReservationStatus.Approved &&
            r.StartTime < reservation.EndTime &&
            r.EndTime > reservation.StartTime);

        if (isTaken) ApiResult.Failure("Slot already taken by another approved booking!");

        reservation.Status = ReservationStatus.Approved;

        var conflicts = await _context.Reservations
            .Where(r => r.ConferenceRoomId == reservation.ConferenceRoomId &&
                        r.Id != id &&
                        r.Status == ReservationStatus.Pending &&
                        r.StartTime < reservation.EndTime &&
                        r.EndTime > reservation.StartTime)
            .ToListAsync();

        foreach (var conflict in conflicts) conflict.Status = ReservationStatus.Rejected;

        await _context.SaveChangesAsync();
        return ApiResult.Success();
    }

    public async Task<bool> DeclineReservationAsync(long id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null) return false;

        reservation.Status = ReservationStatus.Rejected;
        await _context.SaveChangesAsync();
        return true;
    }
}