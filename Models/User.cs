using Microsoft.AspNetCore.Identity;

namespace OfficeMaster.Models;

public class User : IdentityUser<long>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public List<Reservation> Reservations { get; set; } = new();
}