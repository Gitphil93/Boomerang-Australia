namespace Boomerang.Domain;

public class Card
{
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string SiteLetter { get; set; } = string.Empty;
    public int ThrowValue { get; set; }
    public int CatchValue { get; set; }
    public string CollectionType { get; set; } = string.Empty;
    public string AnimalType { get; set; } = string.Empty;
    public string ActivityType { get; set; } = string.Empty;

    public override string ToString() =>
        $"{Name} ({Region} {SiteLetter}) T:{ThrowValue} C:{CatchValue}";
}
