using OfficeMaster.Models;
using OfficeMaster.Models.Enum;
using OfficeMaster.ViewModels.Shared;

namespace OfficeMaster.Interface;

public interface IReservationService
{
    Task<List<Reservation>> GetReservationsAsync(string? search, ReservationStatus? status);
    Task<ApiResult> ApproveReservationAsync(long id);
    Task<bool> DeclineReservationAsync(long id);
}