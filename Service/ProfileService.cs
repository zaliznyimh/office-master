using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeMaster.Data;
using OfficeMaster.Interface;
using OfficeMaster.Models;
using OfficeMaster.ViewModels.Profile;

namespace OfficeMaster.Service;

public class ProfileService : IProfileService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public ProfileService(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<Reservation>> GetUserReservations(long userId)
    {
        return await _context.Reservations
            .Include(r => r.ConferenceRoom)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.StartTime)
            .ToListAsync();
    }

    public async Task<IdentityResult> UpdateUserProfileAsync(long userId, ProfileViewModel model)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) 
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }
        
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.CompanyName = model.CompanyName ?? string.Empty;
        user.Email = model.Email;
        user.UserName = model.Email;
        
        if (model.DateOfBirth.HasValue)
        {
            user.DateOfBirth = DateTime.SpecifyKind(model.DateOfBirth.Value, DateTimeKind.Utc);
        }
        
        return await _userManager.UpdateAsync(user);
    }
}