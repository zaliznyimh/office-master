using System.ComponentModel.DataAnnotations;
using OfficeMaster.Models.Enum;

namespace OfficeMaster.Models;

public class Reservation : BaseEntity
{
    [DataType(DataType.DateTime)]
    public DateTime StartTime { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime EndTime { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal TotalPrice { get; set; }
    
    public ReservationStatus Status { get; set; } 
    
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    
    public long ConferenceRoomId { get; set; }
    public ConferenceRoom ConferenceRoom { get; set; } = null!;
}