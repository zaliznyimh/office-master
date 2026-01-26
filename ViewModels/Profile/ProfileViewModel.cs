using System.ComponentModel.DataAnnotations;
using OfficeMaster.Models;

namespace OfficeMaster.ViewModels.Profile;

public class ProfileViewModel
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public List<Reservation> MyReservations { get; set; } = new();
}