using System.ComponentModel.DataAnnotations;
using OfficeMaster.Models.Enum;

namespace OfficeMaster.ViewModels.Rooms;

public class RoomFormViewModel
{
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [Range(1, 1000)]
    public int Capacity { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal PricePerHour { get; set; }
    
    [Required]
    public RoomType RoomType { get; set; }
    
    public bool HasProjector { get; set; }
    
    [Url]
    [Required(ErrorMessage = "Please enter an image URL")]
    public string? ImageUrl { get; set; } = string.Empty;
}