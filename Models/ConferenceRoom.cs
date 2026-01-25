using System.ComponentModel.DataAnnotations;

namespace OfficeMaster.Models;

public class ConferenceRoom : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Range(1, 300, ErrorMessage = "Capacity must be between 1 and 300")]
    public int Capacity { get; set; }
    
    public bool HasProjector { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal PricePerHour { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public List<Reservation> Reservations { get; set; } = new();
}