using System.Text.Json.Serialization;

namespace Boomerang.Domain;

/// <summary>
/// Represents a card and maps JSON fields to game properties.
/// </summary>

public class Card
{
    public string Name { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    
    [JsonPropertyName("site")]
    public string SiteLetter { get; set; } = string.Empty;

    
    [JsonPropertyName("number")]
    public int Number
    {
        get => ThrowValue;
        set
        {
            ThrowValue = value;
            CatchValue = value;
        }
    }

   
    public int ThrowValue { get; set; }
    public int CatchValue { get; set; }

    
    [JsonPropertyName("collection")]
    public string CollectionType { get; set; } = string.Empty;

    
    [JsonPropertyName("animal")]
    public string AnimalType { get; set; } = string.Empty;

    
    [JsonPropertyName("activity")]
    public string ActivityType { get; set; } = string.Empty;

    public override string ToString() =>
        $"{Name} ({Region} {SiteLetter}) Throw:{ThrowValue} Catch:{CatchValue}";
}
