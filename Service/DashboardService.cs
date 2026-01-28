using Microsoft.EntityFrameworkCore;
using OfficeMaster.Data;
using OfficeMaster.Interface;
using OfficeMaster.Models.Enum;
using OfficeMaster.ViewModels.Dashboard;

namespace OfficeMaster.Service;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDashboardViewModel> GetDashboardDataAsync()
    {
        var model = new AdminDashboardViewModel();
        
        model.TotalRooms = await _context.ConferenceRooms.CountAsync();
        
        model.TotalBookings = await _context.Reservations.CountAsync();
        
        model.PendingCount = await _context.Reservations
            .CountAsync(r => r.Status == ReservationStatus.Pending);
            
        model.TotalRevenue = await _context.Reservations
            .Where(r => r.Status == ReservationStatus.Approved)
            .SumAsync(r => r.TotalPrice);
        
        model.PendingReservations = await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.ConferenceRoom)
            .Where(r => r.Status == ReservationStatus.Pending)
            .OrderBy(r => r.StartTime)
            .ToListAsync();

        return model;
    }
}