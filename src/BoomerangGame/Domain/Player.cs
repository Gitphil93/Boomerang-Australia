using System.Collections.Generic;

namespace Boomerang.Domain;

public class Player
{
    public string Name { get; }

  
    public List<Card> Hand { get; } = new();

   
    public List<Card> Draft { get; } = new();

    // Korten spelaren faktiskt spelade i kronologisk ordning:
    // index 0 = Throw card
    // index 6 = Catch card
    public List<Card> PlayedCards { get; } = new();

    // Scoring state över hela spelet
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
