using System.ComponentModel.DataAnnotations;
using OfficeMaster.Models;

namespace OfficeMaster.ViewModels.Rooms;

public class RoomDetailsViewModel
{
    public ConferenceRoom? Room { get; set; }
    
    [Required]
    public long RoomId { get; set; }

    [Required(ErrorMessage = "Select start time")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Select end time")]
    public DateTime EndTime { get; set; }
}