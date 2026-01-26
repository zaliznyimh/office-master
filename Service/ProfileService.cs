using Microsoft.EntityFrameworkCore;
using OfficeMaster.Data;
using OfficeMaster.Interface;
using OfficeMaster.Models;

namespace OfficeMaster.Service;

public class ProfileService : IProfileService
{
    private readonly ApplicationDbContext _context;

    public ProfileService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetUserReservations(long userId)
    {
        return await _context.Reservations
            .Include(r => r.ConferenceRoom)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.StartTime)
            .ToListAsync();
    }
}