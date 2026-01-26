using Newtonsoft.Json;

namespace OfficeMaster.ViewModels.Shared;

public class CalendarResponse
{
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; } = string.Empty;
    
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonProperty(PropertyName = "start")]
    public DateTime Start { get; set; }
    
    [JsonProperty(PropertyName = "end")]
    public DateTime End { get; set; }
    
    [JsonProperty(PropertyName = "color")]
    public string Color { get; set; } = "#d9534f";
}