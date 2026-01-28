using OfficeMaster.Models;

namespace OfficeMaster.ViewModels.Dashboard;

public class AdminDashboardViewModel
{
    public int TotalRooms { get; set; }
    public int TotalBookings { get; set; }
    public int PendingCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<Reservation> PendingReservations { get; set; } = new();
}