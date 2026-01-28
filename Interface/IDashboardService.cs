using OfficeMaster.ViewModels.Dashboard;

namespace OfficeMaster.Interface;

public interface IDashboardService
{
    Task<AdminDashboardViewModel> GetDashboardDataAsync();
}
