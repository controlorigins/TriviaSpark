using System.Text.Json.Serialization;

namespace JeopardyData;


public class JeopardyQuestion
{
    public required string Category { get; set; }
    [JsonPropertyName("air_date")]
    public required string AirDate { get; set; }
    public required string Question { get; set; }
    public required string Value { get; set; }
    public required string Answer { get; set; }
    public required string Round { get; set; }
    [JsonPropertyName("show_number")] // Maps "show_number" in JSON to this property
    public required string ShowNumber { get; set; }
}