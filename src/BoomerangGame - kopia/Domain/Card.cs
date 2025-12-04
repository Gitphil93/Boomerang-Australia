using System.Text.Json.Serialization;

namespace Boomerang.Domain;

public class Card
{
    public string Name { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    
    [JsonPropertyName("site")]
    public string SiteLetter { get; set; } = string.Empty;

    // JSON: "number"
    // Vi använder det numret som både Throw och Catch-tal
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

    // Interna värden som resten av koden använder
    public int ThrowValue { get; set; }
    public int CatchValue { get; set; }

    // JSON: "collection"
    [JsonPropertyName("collection")]
    public string CollectionType { get; set; } = string.Empty;

    // JSON: "animal"
    [JsonPropertyName("animal")]
    public string AnimalType { get; set; } = string.Empty;

    // JSON: "activity"
    [JsonPropertyName("activity")]
    public string ActivityType { get; set; } = string.Empty;

    public override string ToString() =>
        $"{Name} ({Region} {SiteLetter}) Throw:{ThrowValue} Catch:{CatchValue}";
}
