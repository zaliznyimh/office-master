using OfficeMaster.Models;

namespace OfficeMaster.Interface;

public interface IProfileService
{
    Task<List<Reservation>> GetUserReservations(long userId);
}