using System.Collections.Generic;

namespace Boomerang.Domain;

/// <summary>
/// Holds player data such as hand, played cards and progress.
/// </summary>

public class Player
{
    public string Name { get; }

  
    public List<Card> Hand { get; } = new();

   
    public List<Card> Draft { get; } = new();

    public List<Card> PlayedCards { get; } = new();
    public ScoreSheet ScoreSheet { get; } = new();
    public HashSet<string> VisitedSites { get; } = new();
    public HashSet<string> CompletedRegions { get; } = new();
    public HashSet<string> ScoredActivities { get; } = new();

    public Player(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}
