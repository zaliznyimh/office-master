using Microsoft.AspNetCore.Identity;
using OfficeMaster.Models;
using OfficeMaster.ViewModels.Profile;

namespace OfficeMaster.Interface;

public interface IProfileService
{
    Task<List<Reservation>> GetUserReservations(long userId);
    Task<IdentityResult> UpdateUserProfileAsync(long userId, ProfileViewModel model);
}